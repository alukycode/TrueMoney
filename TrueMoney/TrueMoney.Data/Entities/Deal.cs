using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueMoney.Common.Enums;

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

        public DateTime CreateDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public string Description { get; set; }

        public TimeSpan DealPeriod { get; set; }

        public decimal Amount { get; set; }

        public DealStatus DealStatus { get; set; }

        public decimal InterestRate { get; set; }

        public int PaymentCount { get; set; }
    }
}
