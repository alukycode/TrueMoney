namespace TrueMoney.Infrastructure.Entities
{
    using System;

    /// <summary>
    /// Bank transaction
    /// </summary>
    public class BankTransaction : Entity
    {
        public decimal Amount { get; set; }

        public User From { get; set; }

        public User For { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsClosed { get; set; }

        public bool IsInProgress { get; set; }
    }
}
