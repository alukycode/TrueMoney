namespace TrueMoney.Models.Basic
{
    using System;

    public class PaymentPlanModel
    {
        public int Id { get; set; }

        public int DealId { get; set; }

        public DateTime CreateTime { get; set; }
    }
}