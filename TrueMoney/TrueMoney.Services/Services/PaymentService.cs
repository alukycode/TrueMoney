using TrueMoney.Common.Enums;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml;

    using Bank.BankApi;
    using Bank.BankEntities;
    using Data;
    using TrueMoney.Models;
    using TrueMoney.Services.Interfaces;

    public class PaymentService : IPaymentService
    {
        private readonly IUserService _userService;
        private readonly IDealService _dealService;
        private readonly IBankApi _bankApi;
        private readonly ITrueMoneyContext _context;

        public PaymentService(IUserService userService, IDealService dealService, IBankApi bankApi, ITrueMoneyContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _userService = userService;
            _dealService = dealService;
            _bankApi = bankApi;
            _context = context;
        }

        public PaymentService(ITrueMoneyContext context)
        {
            _context = context;
        }

        public async Task<PaymentResult> LendMoney(VisaPaymentViewModel visaPaymentViewModel, int currentUserId)
        {
            var deal = await _context.Deals
                .Include(x=>x.Owner)
                .FirstAsync(x => x.Id == visaPaymentViewModel.DealId); //тут еще, возможно, нужны какие-то проверки с текущим юзером, но если не нужны, то не добавляйте!!!
            var recipient = deal.Owner;
            var sender = await _context.Users.FirstAsync(x => x.Id == currentUserId);

            if (deal.Amount != visaPaymentViewModel.PaymentCount)
            {
                return PaymentResult.Error;
            }
            if (recipient.Id != visaPaymentViewModel.PayForId)
            {
                return PaymentResult.PermissionError;
            }

            var result = await
                _bankApi.Do(
                    new BankTransaction
                    {
                        Amount = visaPaymentViewModel.PaymentCount,
                        SenderAccountNumber = sender.BankAccountNumber,
                        RecipientAccountNumber = recipient.BankAccountNumber,
                    });

            switch (result)
            {
                case BankResponse.Success:
                    deal.DealStatus = DealStatus.InProgress;
                    var paymentPlan = new PaymentPlan
                                          {
                                              CreateTime = DateTime.Now,
                                              DealId = deal.Id,
                                              Deal = deal
                                          };
                    deal.PaymentPlan = paymentPlan;

                    await _context.SaveChangesAsync();

                    paymentPlan = await _context.PaymentPlans.FirstOrDefaultAsync(x => x.DealId == deal.Id);
                    paymentPlan.Payments = CalculatePayments(deal);

                    await _context.SaveChangesAsync();
                    
                    return PaymentResult.Success;

                case BankResponse.NotEnoughtMoney:
                    return PaymentResult.NotEnoughtMoney;

                default:
                    return PaymentResult.Error;
            }
        }

        public async Task<PaymentResult> Payout(VisaPaymentViewModel visaPaymentViewModel, int currentUserId)
        {
            var deal = await _context.Deals
                .Include(x => x.Owner)
                .Include(x=> x.Offers)
                .FirstAsync(x => x.Id == visaPaymentViewModel.DealId);
            var recipient = deal.Offers.First(x => x.IsApproved).Offerer;

            if (recipient.Id != visaPaymentViewModel.PayForId)
            {
                return PaymentResult.PermissionError;
            }

            var result =
                await
                _bankApi.DoWithVisa(
                    new BankVisaTransaction
                        {
                            Amount = visaPaymentViewModel.PaymentCount,
                            RecipientAccountNumber = recipient.BankAccountNumber,
                            SenderCardNumber = visaPaymentViewModel.CardNumber,
                            SenderCcvCode = visaPaymentViewModel.CvvCode,
                            SenderName = visaPaymentViewModel.Name,
                            SenderValidBefore = visaPaymentViewModel.ValidBefore.ToString("MM/yy")
                        });

            switch (result)
            {
                case BankResponse.Success:
                    //todo - check and calculate liability if need
                    var paymentPlan = await _context.PaymentPlans
                        .Include(x=>x.Payments).FirstOrDefaultAsync(x => x.DealId == deal.Id);
                    var allPaidBefore =
                        paymentPlan.Payments.Where(x => x.IsPaid).Select(x => x.Amount + x.Liability).Sum();
                    //some extra money before previous payment
                    var extraMoney = paymentPlan.BankTransactions.Select(x => x.Amount).Sum() - allPaidBefore;
                    //new money will used to close calculated payments
                    var newMoney = visaPaymentViewModel.PaymentCount + extraMoney;
                    var nearByPayment = paymentPlan.Payments.Where(x => !x.IsPaid).OrderBy(x => x.DueDate).ToList();
                    foreach (var payment in nearByPayment)
                    {
                        var currentPaymentMoney = payment.Amount + payment.Liability;
                        if (currentPaymentMoney <= newMoney)
                        {
                            payment.IsPaid = true;
                            payment.PaidDate = DateTime.Now;
                            newMoney -= currentPaymentMoney;
                        }
                        else
                        {
                            break;
                        }
                    }

                    paymentPlan.BankTransactions.Add(
                        new Data.Entities.BankTransaction
                            {
                                Amount = visaPaymentViewModel.PaymentCount,
                                DateOfPayment = DateTime.Now,
                                PaymentPlan = paymentPlan,
                                PaymentPlanId = paymentPlan.Id
                            });

                    await _context.SaveChangesAsync();

                    return PaymentResult.Success;

                case BankResponse.NotEnoughtMoney:
                    return PaymentResult.NotEnoughtMoney;

                default:
                    return PaymentResult.Error;
            }
        }

        public List<Payment> CalculatePayments(Deal deal)
        {
            var paymentList = new List<Payment>();
            var extraAmount = deal.Amount % deal.PaymentCount;
            var periodAmount = (deal.Amount - extraAmount) / deal.PaymentCount;
            var period = deal.DealPeriod / deal.PaymentCount;
            var extraTime = deal.DealPeriod % deal.PaymentCount;
            var currentDate = DateTime.Now.AddDays(period + extraTime);
            paymentList.Add(new Payment
                                {
                                    Amount = periodAmount+extraAmount,
                                    DueDate = currentDate,
                                    Liability = 0,
                                    PaymentPlan = deal.PaymentPlan,
                                    PaymentPlanId = deal.PaymentPlan.Id
            });
            var number = 1;
            while (number < deal.PaymentCount)
            {
                paymentList.Add(new Payment
                {
                    Amount = periodAmount,
                    DueDate = currentDate.AddDays(period * number++),
                    Liability = 0,
                    PaymentPlan = deal.PaymentPlan,
                    PaymentPlanId = deal.PaymentPlan.Id
                });
            }

            return paymentList;
        }
    }
}