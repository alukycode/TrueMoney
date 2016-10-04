namespace TrueMoney.Infrastructure.Entities
{
    using System;

    public class MoneyApplication
    {
        public int Id { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public User Borrower { get; set; }

        public DateTime CreateDate { get; set; }

        public float Count { get; set; }

        /// <summary>
        /// Percent
        /// </summary>
        public float Rate { get; set; }

        public IEquatable<Offer> Offers { get; set; }

        public bool IsClosed { get; set; }
        public DateTime CloseDate { get; set; }
    }
}
