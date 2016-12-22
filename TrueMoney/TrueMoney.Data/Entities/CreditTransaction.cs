using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueMoney.Data.Entities
{
    public class CreditTransaction : Entity
    {
        [Required]
        public virtual Deal Deal { get; set; }

        public int? RecipientId { get; set; }

        public User Recipient { get; set; }

        public int? SenderId { get; set; }

        public User Sender { get; set; }

        public decimal Amount { get; set; }
    }
}
