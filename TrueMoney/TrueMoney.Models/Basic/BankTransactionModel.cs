namespace TrueMoney.Models.Basic
{
    using System;

    public class BankTransactionModel
    {
        public int PaymentPlanId { get; set; }

        public DateTime DateOfPayment { get; set; }

        public decimal Amount { get; set; }
    }
}