namespace TrueMoney.Models.Basic
{
    using System;
    using System.Collections.Generic;
    using Common.Enums;

    public class DealModel
    {
        public int Id { get; set; }

        //public bool IsOpen { get; set; }

        public decimal InterestRate { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public decimal Amount { get; set; }

        public int DealPeriod { get; set; }

        public string Description { get; set; }

        public DealStatus DealStatus { get; set; }
        //public bool IsWaitForLoan { get; set; }

        //public bool IsClosed { get; set; }

        //public bool IsWaitForApprove { get; set; }

        //public bool IsInProgress { get; set; }

        public int OwnerId { get; set; }

        public string OwnerFullName { get; set; }
    }
}
