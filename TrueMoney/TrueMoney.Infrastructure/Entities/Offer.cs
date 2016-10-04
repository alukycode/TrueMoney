namespace TrueMoney.Infrastructure.Entities
{
    using System;

    public class Offer : BaseEntity
    {
        /// <summary>
        /// User
        /// </summary>
        public User Lender { get; set; }

        public DateTime CreateTime { get; set; }
        
        public bool IsClosed { get; set; }

        public DateTime CloseDate { get; set; }

        public MoneyApplication MoneyApplication { get; set; }
    }
}
