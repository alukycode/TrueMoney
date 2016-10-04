namespace TrueMoney.Infrastructure.Entities
{
    using System.Collections.Generic;

    public class User : BaseEntity
    {
        public string FullName { get; set; }

        public BankAccount BankAccount { get; set; }

        public IList<MoneyApplication> Applications { get; set; }

        public IList<Offer> Offers { get; set; }

        public IList<Loan> Loans { get; set; }
    }
}
