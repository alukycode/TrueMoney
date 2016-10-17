namespace TrueMoney.Infrastructure.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Deal : Entity
    {
        public User Borrower { get; set; }
        public User Lender { get; set; }
        public DealStatus Status { get; set; } = DealStatus.Open;

        public DateTime CreateDate { get; set; }
        public DateTime CloseDate { get; set; }

        public float Count { get; set; }

        public int PaymentCount { get; set; } = 1;

        public float Rate { get; set; }

        public int DayCount { get; set; }

        public IList<Offer> Offers { get; set; } = new List<Offer>();

        public int FinishOfferId { get; set; }

        public string Description { get; set; }

        public bool WaitForApprove { get; set; }

        public PaymentPlan PaymentPlan { get; set; }

        public bool IsClosed
        {
            get
            {
                return Status == DealStatus.Closed;
            }
        }

        public bool IsTakePart(User user)
        {
            return user != null && (Offers.Any(x => x.Lender.Equals(user)) || Equals(this.Borrower, user));
        }
    }
}
