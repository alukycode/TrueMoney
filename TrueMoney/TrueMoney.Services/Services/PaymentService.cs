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

            var result = _bankApi.DoWithVisa(
                new BankVisaTransaction
                {
                    Amount = visaPaymentViewModel.PaymentCount,
                    RecipientAccountNumber = recipient.CardNumber,
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

        public async Task<PaymentResult> Payout(VisaPaymentViewModel visaPaymentViewModel)
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

            if (nearByPayment[0].Amount + nearByPayment[0].Liability - extraMoney > visaPaymentViewModel.PaymentCount)
            {
                return PaymentResult.LessThenMinAmount;
            }

            var result = BankResponse.NotEnoughtMoney;
            var resultMoneyAmount = visaPaymentViewModel.PaymentCount * (1 + NumericConstants.Tax);
            var userBalance = _bankApi.GetBalance(deal.Owner.CardNumber);
            if (!userBalance.HasValue)
            {
                return PaymentResult.Error;
            }

            if (userBalance >= resultMoneyAmount)
            {
                result = _bankApi.DoWithVisa(
                        new BankVisaTransaction
                        {
                            Amount = visaPaymentViewModel.PaymentCount,
                            RecipientAccountNumber = recipient.CardNumber,
                            SenderCardNumber = visaPaymentViewModel.CardNumber,
                            SenderCcvCode = visaPaymentViewModel.CvvCode,
                            SenderName = visaPaymentViewModel.Name,
                            SenderValidBefore = visaPaymentViewModel.ValidBefore
                        });
                _bankApi.DoWithVisa(
                    new BankVisaTransaction
                    {
                        Amount = visaPaymentViewModel.PaymentCount * NumericConstants.Tax,
                        RecipientAccountNumber = BankConstants.TrueMoneyAccountNumber,
                        SenderCardNumber = visaPaymentViewModel.CardNumber,
                        SenderCcvCode = visaPaymentViewModel.CvvCode,
                        SenderName = visaPaymentViewModel.Name,
                        SenderValidBefore = visaPaymentViewModel.ValidBefore
                    }); 
            }

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

                    // check if all payments were paid
                    if (paymentPlan.Payments.All(x => x.IsPaid))
                    {
                        deal.DealStatus = DealStatus.Closed;
                        UpdateRatingAfterFinish(ref deal);
                    }

                    await _context.SaveChangesAsync();

                    return PaymentResult.Success;

                case BankResponse.NotEnoughtMoney:
                    return PaymentResult.NotEnoughtMoney;

                case BankResponse.PermissionError:
                    return PaymentResult.PermissionError;

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
            var amount = deal.Amount * (1 + deal.InterestRate / 100);
            var paymentList = new List<Payment>();
            var extraAmount = amount % deal.PaymentCount;
            var periodAmount = (amount - extraAmount) / deal.PaymentCount;
            var period = deal.DealPeriod / deal.PaymentCount;
            var extraTime = deal.DealPeriod % deal.PaymentCount;
            var firstPayDate = DateTime.Now.AddDays(period + extraTime);
            var payment = new Payment
            {
                Amount = periodAmount + extraAmount,
                DueDate = firstPayDate
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

        public void UpdateRatingAfterFinish(ref Deal deal)
        {
            var mainOfferer = deal.Offers.First(x => x.IsApproved).Offerer;
            deal.Owner.Rating += Rating.SuccessFinishDeal;
            mainOfferer.Rating += Rating.SuccessFinishDeal;

            var allPaidInTime = true;

            for (int i = 0; i < deal.PaymentPlan.Payments.Count; i++)
            {
                var currentPayment = deal.PaymentPlan.Payments[i];
                if (currentPayment.DueDate < currentPayment.PaidDate)
                {
                    deal.Owner.Rating += Rating.DelayPayment;
                    allPaidInTime = false;
                }
            }

            if (allPaidInTime)
            {
                deal.Owner.Rating += Rating.SuccessPayments;
            }
        }
    }
}