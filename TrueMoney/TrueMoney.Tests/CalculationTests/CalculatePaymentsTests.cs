namespace TrueMoney.Tests.CalculationTests
{
    using System;
    using System.Linq;

    using Moq;

    using NUnit.Framework;

    using TrueMoney.Data;
    using TrueMoney.Data.Entities;
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
            // Create fake data
            var deal = new Deal
                                    {
                                        Amount = amount,
                                        DealPeriod = period,
                                        PaymentCount = paymentCount,
                                        PaymentPlan = new PaymentPlan { CreateTime = DateTime.Now },
                                        PaymentPlanId = 1
            };

            // Create mock unit of work
            var mockData = new Mock<ITrueMoneyContext>();

            // Setup service
            var paymentService = new PaymentService(mockData.Object);

            // Invoke
            var result = paymentService.CalculatePayments(deal);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(result.Count, paymentCount);
            Assert.AreEqual(result[result.Count - 1].DueDate.Date, DateTime.Now.AddDays(period).Date);
            Assert.AreEqual(result.Sum(x=>x.Amount), amount);
        }
    }
}