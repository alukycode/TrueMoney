namespace TrueMoney.Tests.BankTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bank.BankApi;
    using Bank.BankEntities;
    using Bank.Resources;

    using Moq;

    using NUnit.Framework;

    using TrueMoney.Data;

    [TestFixture]
    public class OperationsTest
    {
        [Test]
        public void TestOperations()
        {
            var accounts = new List<BankAccount>();
            decimal totalSum = 0;
            for (int i = 0; i < 5; i++)
            {
                accounts.Add(
                    new BankAccount
                    {
                        Id = i,
                        BankAccountNumber = $"408.17.810.0.9991.{i.ToString("D6")}",
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
            Assert.AreEqual(result.Count, 5);
            CheckTotalSum(totalSum, result);

            var bankVisaTransaction1 = new BankVisaTransaction
            {
                Amount = 1000,
                RecipientAccountNumber = accounts[0].BankAccountNumber,
                SenderCardNumber = accounts[1].VisaNumber,
                SenderCcvCode = accounts[1].VisaCcv,
                SenderName = accounts[1].VisaName,
                SenderValidBefore = accounts[1].VisaDate
            };


            var bankApi = new BankApi();
            var res1 = bankApi.DoWithVisa(bankVisaTransaction1).Result;
            Assert.NotNull(res1);
            result = BankDataHelper.GetAccounts();
            Assert.AreEqual(res1, BankResponse.Success);
            CheckTotalSum(totalSum, result);
            Assert.AreEqual(1000, result[0].Amount);
            Assert.AreEqual(0, result[1].Amount);
        }

        private static void CheckTotalSum(decimal totalSum, List<BankAccount> accounts)
        {
            Assert.AreEqual(totalSum, accounts.Select(x => x.Amount).Sum());
        }
    }
}