namespace TrueMoney.Infrastructure.Entities
{
    using System.Collections.Generic;

    public class User : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public Passport Passport { get; set; }
        
        public bool IsActive { get; set; }

        public string AspUserId { get; set; }

        public string AccountNumber { get; set; }

        public bool IsHaveOpenDealOrLoan { get; set; }

        public int Rating { get; set; }

        public IList<Deal> Deals { get; set; }

        public IList<Offer> Offers { get; set; }
    }
}
