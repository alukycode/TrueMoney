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
    public class PaymentServiceTests
    {
        [Test]
        [TestCase(1000, 7, 1)]
        [TestCase(1000, 7, 2)]
        [TestCase(1000, 7, 7)]
        [TestCase(999, 7, 7)]
        [TestCase(999.99, 20, 5)]
        public void CalculatePaymentsTest(decimal amount, int period, int paymentCount)
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
    }
}