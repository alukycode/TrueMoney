namespace Bank.Resources
{
    using System;
    using System.Linq;
    using System.Xml.Linq;

    /// <summary>
    /// Generate default test data.
    /// </summary>
    public static class BankDataHelper
    {
        public static string AccountKey = "account";
        public static string IdAttrKey = "id";
        public static string BankAccountNumberAttrKey = "bankAccountNumber";
        public static string AmountAttrKey = "amount";
        public static string VisaNumberAttrKey = "visaNumber";
        public static string VisaNameAttrKey = "visaName";
        public static string VisaCcvAttrKey = "visaCcv";
        public static string VisaDateAttrKey = "visaDate";

        public static void UpdateDataFile()
        {
            //https://ru.wikipedia.org/wiki/%D0%A0%D0%B0%D1%81%D1%87%D1%91%D1%82%D0%BD%D1%8B%D0%B9_%D1%81%D1%87%D1%91%D1%82
            var xDoc = new XDocument();
            var root = new XElement("accounts");
            for (int i = 0; i < 10; i++)
            {
                root.Add(
                    CreateAccount(
                        i.ToString(),
                        $"408.17.810.0.9991.{i.ToString("D6")}",
                        1000f,
                        i.ToString("D16"),
                        $"Test User{i}",
                        i.ToString("D3"),
                        "01/18"));
            }
            xDoc.Add(root);
            xDoc.Save("BankData.xml");
        }

        public static XElement CreateAccount(string id, string bankNumber, float amount, string visaNumber,
            string visaName, string visaCcv, string visaDate)
        {
            var xElement = new XElement(AccountKey);
            xElement.Add(new XAttribute(IdAttrKey, id));
            xElement.Add(new XAttribute(BankAccountNumberAttrKey, bankNumber));
            xElement.Add(new XAttribute(AmountAttrKey, amount));
            xElement.Add(new XAttribute(VisaNumberAttrKey, visaNumber));
            xElement.Add(new XAttribute(VisaNameAttrKey, visaName));
            xElement.Add(new XAttribute(VisaCcvAttrKey, visaCcv));
            xElement.Add(new XAttribute(VisaDateAttrKey, visaDate));

            return xElement;
        }
    }
}
