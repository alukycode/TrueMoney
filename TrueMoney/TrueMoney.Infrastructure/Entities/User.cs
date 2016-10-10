namespace TrueMoney.Infrastructure.Entities
{
    using System;
    using System.Collections.Generic;

    public class User : Entity
    {
        public string Name { get; set; }

        public BankAccount BankAccount { get; set; }

        public IList<MoneyApplication> Applications { get; set; }

        public IList<Offer> Offers { get; set; }

        public IList<Loan> Loans { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FamilyName { get; set; }
        
        public string PassportSeria { get; set; }
        public string PassportNumber { get; set; }
        public DateTime PassportGiveTime { get; set; }
        public string PassportGiveOrganisation { get; set; }

        public bool IsActive { get; set; }

        public override bool Equals(object o)
        {
            var otherUser = o as User;
            return otherUser != null && this.Id == otherUser.Id;
        }

        public override int GetHashCode()
        {
            return 1000 + this.Id;
        }
    }
}
