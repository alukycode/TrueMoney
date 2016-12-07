namespace Bank.Resources
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using Bank.BankEntities;
    using TrueMoney.Common;

    /// <summary>
    /// Generate default test data.
    /// </summary>
    public static class BankDataHelper
    {
        private static string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BankData.xml");

        public static void UpdateDataFile()
        {
            //https://ru.wikipedia.org/wiki/%D0%A0%D0%B0%D1%81%D1%87%D1%91%D1%82%D0%BD%D1%8B%D0%B9_%D1%81%D1%87%D1%91%D1%82
            var accounts = new List<BankAccount>();
            accounts.Add(new BankAccount
            {
                Id = 0,
                BankAccountNumber = BankConstants.TrueMoneyAccountNumber,
                Amount = 1000M,
                VisaNumber = "-",
                VisaName = "-",
                VisaCcv = "-",
                VisaDate = "-"
            });
            for (int i = 1; i < 11; i++)
            {
                var crutch = i - 1;
                accounts.Add(
                    new BankAccount
                    {
                        Id = i,
                        BankAccountNumber = $"408.17.810.0.9991.{crutch.ToString("D6")}",
                        Amount = i * 1000,
                        VisaNumber = crutch.ToString("D16"),
                        VisaName = $"Test User{crutch}",
                        VisaCcv = crutch.ToString("D3"),
                        VisaDate = "01/18"
                    });
            }

            SaveAccounts(accounts);
        }

        public static List<BankAccount> GetAccounts()
        {
            if (!File.Exists(_filePath))
            {
                UpdateDataFile();
            }

            List<BankAccount> accounts;
            XmlSerializer formatter = new XmlSerializer(typeof(List<BankAccount>));
            using (FileStream fs = new FileStream(_filePath, FileMode.Open))
            {
                accounts = (List<BankAccount>)formatter.Deserialize(fs);
            }

            return accounts;
        }
        
        public static void SaveAccounts(List<BankAccount> accounts)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<BankAccount>));
            using (FileStream fs = new FileStream(_filePath, FileMode.Create))
            {
                formatter.Serialize(fs, accounts);
            }
        }
    }
}
