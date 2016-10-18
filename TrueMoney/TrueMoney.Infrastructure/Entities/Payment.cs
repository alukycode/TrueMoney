namespace TrueMoney.Infrastructure.Entities
{
    using System;

    /// <summary>
    /// Time and amount to pay
    /// </summary>
    public class Payment : Entity
    {
        public DateTime TimeToPay { get; set; }

        public DateTime PayTime { get; set; }

        public decimal Amount { get; set; }

        /// <summary>
        /// Extra money if borrower did not pay in time
        /// </summary>
        public decimal Liability { get; set; }

        /// <summary>
        /// Borrower
        /// </summary>
        public User From { get; set; }

        /// <summary>
        /// Lender
        /// </summary>
        public User For { get; set; }

        public Deal Deal { get; set; }

        public bool IsPayed { get; set; }
    }
}
