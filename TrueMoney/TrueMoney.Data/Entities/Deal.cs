using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueMoney.Infrastructure.Entities;

namespace TrueMoney.Data.Entities
{
    public class Deal : Entity
    {
        public int OwnerId { get; set; }

        [Required]
        public User Owner { get; set; }

        public List<Offer> Offers { get; set; }

        public int? PaymentPlanId { get; set; }

        public PaymentPlan PaymentPlan { get; set; }

        public bool IsApproved { get; set; }

        public DateTime CreateDate { get; set; }

        public string Description { get; set; }

        public TimeSpan DealPeriod { get; set; }

        public decimal Amount { get; set; }

        public DealStatus DealStatus { get; set; }

        public int InterestRate { get; set; }
    }
}
