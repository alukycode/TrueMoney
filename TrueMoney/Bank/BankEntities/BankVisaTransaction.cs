namespace Bank.BankEntities
{
    using System;

    public class BankVisaTransaction
    {
        public BankVisaTransaction()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string SenderCardNumber { get; set; }

        public string SenderValidBefore { get; set; }

        public string SenderName { get; set; }

        public string SenderCcvCode { get; set; }

        public string RecipientAccountNumber { get; set; }

        public decimal Amount { get; set; }
    }
}