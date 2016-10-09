namespace TrueMoney.Infrastructure.Entities
{
    using System;

    /// <summary>
    /// Bank transaction
    /// </summary>
    public class BankTransaction : Entity
    {
        public float Amount { get; set; }

        public BankAccount From { get; set; }

        public BankAccount For { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsClosed { get; set; }

        public bool IsInProgress { get; set; }
    }
}
