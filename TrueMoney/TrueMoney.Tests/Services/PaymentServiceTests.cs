namespace TrueMoney.Tests.CalculationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Moq;

    using NUnit.Framework;

    using TrueMoney.Common.Enums;
    using TrueMoney.Data;
    using TrueMoney.Data.Entities;
    using TrueMoney.Services.Extensions;
    using TrueMoney.Services.Helpers;
    using TrueMoney.Services.Services;

    [TestFixture]
    public class PaymentServiceTests
    {
        [Test]
        [TestCase(1000, 7, 1, 20)]
        [TestCase(1000, 7, 2, 2)]
        [TestCase(1000, 7, 7, 15)]
        [TestCase(999, 7, 7, 20)]
        [TestCase(999.99, 20, 5, 30)]
        public void CalculatePaymentsTest(decimal amount, int period, int paymentCount,
            decimal interestRate)
        {
            var deal = new Deal
            {
                Amount = amount,
                DealPeriod = period,
                PaymentCount = paymentCount,
                PaymentPlan = new PaymentPlan { CreateTime = DateTime.Now, Id = 1 },
                InterestRate = interestRate
            };
            var mockData = new Mock<ITrueMoneyContext>();
            var paymentService = new PaymentService(mockData.Object);
            var result = paymentService.CalculatePayments(deal);
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, paymentCount);
            Assert.AreEqual(result[result.Count - 1].DueDate.Date, DateTime.Now.AddDays(period).Date);
            Assert.IsTrue(Math.Abs(result.Sum(x => x.Amount) - amount * (1 + interestRate / 100)) < NumericConstants.Eps);
        }

        [Test]
        [TestCase(1, new bool[] { }, 8)]
        [TestCase(1, new bool[] { true }, 8)]
        [TestCase(1, new bool[] { true, true }, 8)]
        [TestCase(-1, new bool[] { false }, 2)]
        [TestCase(6, new bool[] { false, false }, 7)]
        [TestCase(1, new bool[] { false, false, false, false }, -2)]
        [TestCase(1, new bool[] { false, true, false, false }, 0)]
        public void UpdateRatingAfterFinishTest(int initialRating, bool[] isInTime, int resultRating)
        {
            var deal = new Deal
                           {
                               Owner = new User { Rating = initialRating },
                               PaymentPlan =
                                   new PaymentPlan
                                       {
                                           CreateTime = DateTime.Now,
                                           Id = 1,
                                           Payments = new List<Payment>()
                                       },
                               DealStatus = DealStatus.Closed,
                               Offers =
                                   new List<Offer>
                                       {
                                           new Offer
                                               {
                                                   IsApproved = true,
                                                   Offerer = new User { Rating = 5 }
                                               }
                                       }
                           };

            for (int i = 0; i < isInTime.Length; i++)
            {
                if (isInTime[i])
                {
                    deal.PaymentPlan.Payments.Add(
                        new Payment { IsPaid = true, PaidDate = DateTime.Now.AddDays(-5), DueDate = DateTime.Now });
                }
                else
                {
                    deal.PaymentPlan.Payments.Add(
                        new Payment { IsPaid = true, PaidDate = DateTime.Now, DueDate = DateTime.Now.AddDays(-5) });
                }
            }

            var mockData = new Mock<ITrueMoneyContext>();
            var paymentService = new PaymentService(mockData.Object);
            paymentService.UpdateRatingAfterFinish(ref deal);

            Assert.AreEqual(deal.Owner.Rating, resultRating);
            Assert.AreEqual(deal.Offers.First(x => x.IsApproved).Offerer.Rating, 10);
        }
    }
}