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
    using Common;
    using Data;
    using TrueMoney.Models;
    using TrueMoney.Services.Extensions;
    using TrueMoney.Services.Interfaces;

    public class PaymentService : IPaymentService
    {
        private readonly IBankApi _bankApi;
        private readonly ITrueMoneyContext _context;

        public PaymentService(IUserService userService, IDealService dealService, IBankApi bankApi, ITrueMoneyContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

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
                .FirstAsync(x => x.Id == visaPaymentViewModel.DealId);
            var recipient = deal.Owner;
            var sender = await _context.Users.FirstAsync(x => x.Id == currentUserId);

            if (Math.Abs(deal.Amount - visaPaymentViewModel.PaymentCount) > NumericConstants.Eps)
            {
                return PaymentResult.Error;
            }

            var result = await
                _bankApi.DoWithVisa(
                    new BankVisaTransaction
                    {
                        Amount = visaPaymentViewModel.PaymentCount,
                        RecipientAccountNumber = recipient.BankAccountNumber,
                        SenderCardNumber = visaPaymentViewModel.CardNumber,
                        SenderCcvCode = visaPaymentViewModel.CvvCode,
                        SenderName = visaPaymentViewModel.Name,
                        SenderValidBefore = visaPaymentViewModel.ValidBefore
                    });

            switch (result)
            {
                case BankResponse.Success:
                    deal.DealStatus = DealStatus.InProgress;
                    deal.PaymentPlan = GeneratePlan(deal);
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
                .Include(x => x.Offers)
                .FirstAsync(x => x.Id == visaPaymentViewModel.DealId);
            var recipient = deal.Offers.First(x => x.IsApproved).Offerer;

            var paymentPlan = await _context
                .PaymentPlans
                .FirstOrDefaultAsync(x => x.Id == deal.Id);
            var allPaidBefore = paymentPlan
                .Payments
                .Where(x => x.IsPaid)
                .Select(x => x.Amount + x.Liability)
                .Sum();
            //some extra money before previous payment
            var extraMoney = paymentPlan.BankTransactions.Select(x => x.Amount).Sum() - allPaidBefore;
            var nearByPayment = paymentPlan.Payments.Where(x => !x.IsPaid)
                .CalculateLiability(extraMoney, deal).OrderBy(x => x.DueDate).ToList();

            if (nearByPayment[0].Amount + nearByPayment[0].Liability > visaPaymentViewModel.PaymentCount)
            {
                return PaymentResult.LessThenMinAmount;
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
                        SenderValidBefore = visaPaymentViewModel.ValidBefore
                    });

            switch (result)
            {
                case BankResponse.Success:
                    //new money will used to close calculated payments
                    var newMoney = visaPaymentViewModel.PaymentCount + extraMoney;
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

        private PaymentPlan GeneratePlan(Deal deal)
        {
            var paymentPlan = new PaymentPlan
            {
                CreateTime = DateTime.Now,
                Payments = CalculatePayments(deal),
            };

            return paymentPlan;
        }

        public List<Payment> CalculatePayments(Deal deal)
        {
            var amount = deal.Amount * deal.InterestRate;
            var paymentList = new List<Payment>();
            var extraAmount = amount % deal.PaymentCount;
            var periodAmount = (amount - extraAmount) / deal.PaymentCount;
            var period = deal.DealPeriod / deal.PaymentCount;
            var extraTime = deal.DealPeriod % deal.PaymentCount;
            var firstPayDate = DateTime.Now.AddDays(period + extraTime);
            var payment = new Payment
            {
                Amount = periodAmount + extraAmount,
                DueDate = firstPayDate,
            };
            paymentList.Add(payment);

            for (int i = 1; i < deal.PaymentCount; i++)
            {
                payment = new Payment
                {
                    Amount = periodAmount,
                    DueDate = firstPayDate.AddDays(period * i),
                };
                paymentList.Add(payment);
            }

            return paymentList;
        }
    }
}