namespace TrueMoney.Tests.Helpers
{
    using System;

    using NUnit.Framework;

    using TrueMoney.Common;
    using TrueMoney.Data.Entities;
    using TrueMoney.Services.Helpers;

    [TestFixture]
    public class PaymentHelperTests
    {
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

            Assert.IsTrue(Math.Abs(result - liability) < NumericConstants.Eps);
        }
    }
}