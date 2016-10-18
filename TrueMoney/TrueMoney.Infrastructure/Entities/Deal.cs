using TrueMoney.Infrastructure.Enums;

namespace TrueMoney.Infrastructure.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Deal : Entity
    {
        public User Owner { get; set; }

        public DealStatus Status { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime CloseDate { get; set; }

        public int InterestRate { get; set; }

        public IList<Offer> Offers { get; set; } = new List<Offer>();

        public string Description { get; set; }

        public TimeSpan DealPeriod { get; set; }

        public PaymentPlan PaymentPlan { get; set; }

        public decimal Amount { get; set; }
    }
}
