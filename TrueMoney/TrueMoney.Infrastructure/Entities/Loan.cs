namespace TrueMoney.Infrastructure.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Finished application
    /// </summary>
    public class Loan : Entity
    {
        /// <summary>
        /// User
        /// </summary>
        public User Lender { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public User Borrower { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsClosed { get; set; }

        public DateTime CloseDate { get; set; }

        public MoneyApplication MoneyApplication { get; set; }

        /// <summary>
        /// List of payments
        /// </summary>
        public IList<Payment> Payments { get; set; } 
    }
}
