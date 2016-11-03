using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueMoney.Data.Entities
{
    public class Payment : Entity
    {
        public int PaymentPlanId { get; set; }

        [Required]
        public virtual PaymentPlan PaymentPlan { get; set; }

        public DateTime DueDate { get; set; }

        public decimal Amount { get; set; }

        public decimal Liability { get; set; }

        public bool IsPaid { get; set; }

        public DateTime? PaidDate { get; set; }
    }
}
