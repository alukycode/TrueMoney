namespace TrueMoney.Tests.BankTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bank.BankEntities;
    using Bank.Resources;

    using NUnit.Framework;

    [TestFixture]
    public class WriteReadTest
    {
        //[Test]
        //[TestCase(10)]
        //[TestCase(1)]
        //[TestCase(5)]
        //[TestCase(2)]
        public void WriteRead(int count)
        {
            var accounts = new List<BankAccount>();
            decimal totalSum = 0;
            for (int i = 0; i < count; i++)
            {
                accounts.Add(
                    new BankAccount
                        {
                            Id = i,
                            CardNumber = $"408.17.810.0.9991.{i.ToString("D6")}",
                            Amount = Convert.ToDecimal(i * 1000),
                            VisaNumber = i.ToString("D16"),
                            VisaName = $"Test User{i}",
                            VisaCcv = i.ToString("D3"),
                            VisaDate = "01/18"
                        });

                totalSum += Convert.ToDecimal(i * 1000);
            }

            BankDataHelper.SaveAccounts(accounts);
            var result = BankDataHelper.GetAccounts();
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, count);
            Assert.AreEqual(totalSum, accounts.Select(x => x.Amount).Sum());
        }
    }
}