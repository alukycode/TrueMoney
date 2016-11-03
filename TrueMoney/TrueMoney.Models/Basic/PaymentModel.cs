namespace TrueMoney.Models.Basic
{
    using System;

    public class PaymentModel
    {
        public int Id { get; set; }

        public int PaymentPlanId { get; set; }

        public DateTime DueDate { get; set; }

        public decimal Amount { get; set; }

        public decimal Liability { get; set; }

        public bool IsPaid { get; set; }

        public DateTime? PaidDate { get; set; }
    }
}