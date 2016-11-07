namespace Bank.BankEntities
{
    using System;

    [Serializable]
    public class BankAccount
    {
        public BankAccount() { }
        public int Id { get; set; } 
        public string BankAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public string VisaNumber { get; set; }
        public string VisaName { get; set; }
        public string VisaCcv { get; set; }
        public string VisaDate { get; set; }
    }
}