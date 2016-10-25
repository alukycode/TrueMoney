namespace TrueMoney.Models.Basic
{
    using System;
    using System.Collections.Generic;

    public class DealModel
    {
        public int Id { get; set; }

        public bool IsOpen { get; set; }

        public decimal Rate { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public decimal Amount { get; set; }

        public int DayCount { get; set; }

        public string Description { get; set; }

        public bool IsWaitForLoan { get; set; }
        
        public bool IsClosed { get; set; }

        public bool IsWaitForApprove { get; set; }

        public bool IsInProgress { get; set; }

        public UserModel Borrower { get; set; }

        public string BorrowerFullName { get; set; }
    }
}
