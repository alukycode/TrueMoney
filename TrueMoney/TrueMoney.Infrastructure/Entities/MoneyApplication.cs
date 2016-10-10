namespace TrueMoney.Infrastructure.Entities
{
    using System;
    using System.Collections.Generic;

    public class MoneyApplication : Entity
    {
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

        public IEnumerable<Offer> Offers { get; set; }

#region finishData
        public bool IsClosed { get; set; }

        public int FinishLoadId { get; set; }

        public int FinishOfferId { get; set; }

#endregion

        public DateTime CloseDate { get; set; }

        public string Description { get; set; }

        public bool WaitForApprove { get; set; }
    }
}
