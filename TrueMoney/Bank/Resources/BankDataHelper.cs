namespace Bank.Resources
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using Bank.BankEntities;

    /// <summary>
    /// Generate default test data.
    /// </summary>
    public static class BankDataHelper
    {
        public static void UpdateDataFile()
        {
            //https://ru.wikipedia.org/wiki/%D0%A0%D0%B0%D1%81%D1%87%D1%91%D1%82%D0%BD%D1%8B%D0%B9_%D1%81%D1%87%D1%91%D1%82
            var accounts = new List<BankAccount>();
            for (int i = 0; i < 10; i++)
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
            }

            SaveAccounts(accounts);
        }

        public static List<BankAccount> GetAccounts()
        {
            List<BankAccount> accounts;
            XmlSerializer formatter = new XmlSerializer(typeof(List<BankAccount>));
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "BankData.xml");
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                accounts = (List<BankAccount>)formatter.Deserialize(fs);
            }

            return accounts;
        }

        public static void SaveAccounts(List<BankAccount> accounts)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<BankAccount>));
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", "BankData.xml");
            using (FileStream fs = new FileStream(filePath, FileMode.Truncate))
            {
                formatter.Serialize(fs, accounts);
            }
        }
    }
}
