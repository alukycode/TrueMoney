namespace TrueMoney.Infrastructure.Entities
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Finished application
    /// </summary>
    public class PaymentPlan : Entity
    {
        public Deal Deal { get; set; }

        public List<Payment> Payments { get; set; }

        public List<BankTransaction> BankTransactions { get; set; } 
        
        public DateTime CreateTime { get; set; }

        public DateTime CloseDate { get; set; }

        public float ExtraCount { get; set; }
    }
}
