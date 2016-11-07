using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueMoney.Data.Entities
{
    public class PaymentPlan : Entity
    {
        public int DealId { get; set; }

        [Required]
        public virtual Deal Deal { get; set; }

        public virtual List<Payment> Payments { get; set; }

        public virtual List<BankTransaction> BankTransactions { get; set; } = new List<BankTransaction>();

        public DateTime CreateTime { get; set; }
    }
}
