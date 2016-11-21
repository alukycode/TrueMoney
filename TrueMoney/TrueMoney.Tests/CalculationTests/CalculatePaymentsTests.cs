namespace TrueMoney.Tests.CalculationTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using Moq;

    using NUnit.Framework;

    using TrueMoney.Data;
    using TrueMoney.Data.Entities;
    using TrueMoney.Services.Extensions;
    using TrueMoney.Services.Helpers;
    using TrueMoney.Services.Services;

    [TestFixture]
    public class CalculatePaymentsTests
    {
        [Test]
        [TestCase(1000, 7, 1)]
        [TestCase(1000, 7, 2)]
        [TestCase(1000, 7, 7)]
        [TestCase(999, 7, 7)]
        [TestCase(999.99, 20, 5)]
        public void CalculateTest(decimal amount, int period, int paymentCount)
        {
            var deal = new Deal
            {
                Amount = amount,
                DealPeriod = period,
                PaymentCount = paymentCount,
                PaymentPlan = new PaymentPlan { CreateTime = DateTime.Now, Id = 1 }
            };
            var mockData = new Mock<ITrueMoneyContext>();
            var paymentService = new PaymentService(mockData.Object);
            var result = paymentService.CalculatePayments(deal);
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, paymentCount);
            Assert.AreEqual(result[result.Count - 1].DueDate.Date, DateTime.Now.AddDays(period).Date);
            Assert.AreEqual(result.Sum(x => x.Amount), amount);
        }

        [Test]
        [TestCase(100, 0, 14, 0, 10, 0)]
        [TestCase(100, 14, 20, 0, 10, 0)]
        [TestCase(100, 0, 20, -5, 10, 0)]
        [TestCase(100, 0, 14, 1, 10, 0.72)]
        [TestCase(995, 200, 18, 4, 19, 33.57)]
        public void CalculateLiabilityTest(
            decimal amount,
            decimal extraMoney,
            int days, 
            int daysDelay,
            decimal interestRate, 
            decimal liability)
        {
            var payment = new Payment
            {
                Amount = amount,
                DueDate = DateTime.Now.AddDays(-daysDelay)
            };
            var result = PaymentHelper.CalculateLiability(payment, extraMoney, interestRate, days);
            var result2 = //Димон, надо пересмотреть этот тест, скорее всего здесь должно быть 2 теста
                new List<Payment> { payment }.CalculateLiability(
                    extraMoney,
                    new Deal { InterestRate = interestRate, DealPeriod = days })
                    .First()
                    .Liability;

            Assert.IsTrue(Math.Abs(result - liability) < NumericConstants.Eps);
            Assert.IsTrue(Math.Abs(result2 - liability) < NumericConstants.Eps);
        }
    }
}