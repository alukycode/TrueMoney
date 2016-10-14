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

        public bool IsStarted { get; set; }
        public bool IsWaitForMoney { get; set; } = true;
        public bool IsClosed { get; set; }

        public DateTime CloseDate { get; set; }

        public Deal Deal { get; set; }

        /// <summary>
        /// List of payments
        /// </summary>
        public IList<Payment> Payments { get; set; }

        public float Count { get; set; }
        public float Rate { get; set; }

        public bool IsTakePart(User user)
        {
            return user != null && (Equals(this.Lender, user) || Equals(this.Borrower, user));
        }
    }
}
