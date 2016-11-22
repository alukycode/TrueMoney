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
        [TestCase(1000, 7, 1, 0.2)]
        [TestCase(1000, 7, 2, 0.02)]
        [TestCase(1000, 7, 7, 0.15)]
        [TestCase(999, 7, 7, 0.2)]
        [TestCase(999.99, 20, 5, 0.3)]
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
            Assert.IsTrue(Math.Abs(result.Sum(x => x.Amount) -amount * interestRate) < NumericConstants.Eps);
        }
    }
}