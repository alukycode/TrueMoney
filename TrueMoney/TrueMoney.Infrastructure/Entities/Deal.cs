namespace TrueMoney.Infrastructure.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Deal : Entity
    {
        public User Borrower { get; set; }

        public DateTime CreateDate { get; set; }

        public float Count { get; set; }

        public int PaymentCount { get; set; } = 1;

        public float Rate { get; set; }

        public int DayCount { get; set; }

        public IList<Offer> Offers { get; set; } = new List<Offer>();

        public bool IsClosed { get; set; }

        public int FinishLoadId { get; set; }

        public int FinishOfferId { get; set; }

        public DateTime CloseDate { get; set; }

        public string Description { get; set; }

        public bool WaitForApprove { get; set; }

        public bool IsTakePart(User user)
        {
            return user != null && (Offers.Any(x => x.Lender.Equals(user)) || Equals(this.Borrower, user));
        }
    }
}
