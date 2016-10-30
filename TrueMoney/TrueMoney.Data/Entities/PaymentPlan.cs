using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public virtual List<BankTransaction> BankTransactions { get; set; } //Если транзакций не будет в базе, то при добавлении первой транзации вылетит налреференс, потому что коллекция будет пустой, так что инициализировать ее нужно в конструкторе, ага

        public DateTime CreateTime { get; set; }
    }
}
