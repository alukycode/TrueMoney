using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueMoney.Common.Enums;

namespace TrueMoney.Data.Entities
{
    public class Deal : Entity
    {
        public int OwnerId { get; set; }

        public virtual User Owner { get; set; }

        [InverseProperty("Deal")]
        public virtual List<Offer> Offers { get; set; } = new List<Offer>();

        public virtual PaymentPlan PaymentPlan { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public string Description { get; set; }

        public int DealPeriod { get; set; }

        public decimal Amount { get; set; }

        public DealStatus DealStatus { get; set; }

        public decimal InterestRate { get; set; }

        public int PaymentCount { get; set; }
    }
}
