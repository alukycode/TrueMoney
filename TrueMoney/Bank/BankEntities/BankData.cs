namespace Bank.BankEntities
{
    using System.Xml.Linq;

    public class BankData
    {
        private string fileName = "BankData.xml";

        /// <summary>
        /// Return xml file with bank data
        /// </summary>
        /// <returns></returns>
        public XDocument GetBankData()
        {
            return new XDocument(this.fileName);
        }
    }
}
