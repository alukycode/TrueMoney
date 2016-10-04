namespace Bank.Resources
{
    using System.Xml.Linq;

    /// <summary>
    /// Generate default test data.
    /// </summary>
    public static class BankDataHelper
    {
        public static string AccountKey = "account";
        public static string IdAttrKey = "id";
        public static string SecretAttrKey = "secret";
        public static string AmountAttrKey = "amount";

        public static void UpdateDataFile()
        {
            var xDoc = new XDocument();
            xDoc.Add(CreateAccount("1", "1111-0000-3333", 1000f));
            xDoc.Add(CreateAccount("2", "2222-0000-3333", 2000f));
            xDoc.Add(CreateAccount("3", "3333-0000-3333", 3000f));
            xDoc.Add(CreateAccount("4", "4444-0000-3333", 4000f));
            xDoc.Add(CreateAccount("5", "5555-0000-3333", 5000f));
            xDoc.Add(CreateAccount("6", "6666-0000-3333", 6000f));
            xDoc.Add(CreateAccount("7", "7777-0000-3333", 7000f));

            xDoc.Save("BankData.xml");
        }

        public static XElement CreateAccount(string id, string secret, float amount)
        {
            var xElement = new XElement(AccountKey);
            xElement.Add(new XAttribute(IdAttrKey, id));
            xElement.Add(new XAttribute(SecretAttrKey, secret));
            xElement.Add(new XAttribute(AmountAttrKey, amount));

            return xElement;
        }
    }
}
