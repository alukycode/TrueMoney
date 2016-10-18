namespace TrueMoney.Infrastructure.Entities
{
    using System;

    public class Offer : Entity
    {
        public User Offerer { get; set; }

        public DateTime CreateTime { get; set; }
        
        public bool IsApproved { get; set; }

        public Deal Deal { get; set; }
        
        public int InterestRate { get; set; }
    }
}
