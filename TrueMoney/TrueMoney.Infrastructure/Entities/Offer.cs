namespace TrueMoney.Infrastructure.Entities
{
    using System;

    public class Offer : Entity
    {
        /// <summary>
        /// User
        /// </summary>
        public User Lender { get; set; }

        public DateTime CreateTime { get; set; }
        
        public bool IsClosed { get; set; }

        public DateTime CloseDate { get; set; }

        public Deal Deal { get; set; }
        
        public bool WaitForApprove { get; set; }
        public float Rate { get; set; }
    }
}
