namespace Bank.BankEntities
{
    using System;

    public class BankTransaction //todo - check bank transaction real data
    {
        public BankTransaction()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string SenderAccountNumber { get; set; }

        public string RecipientAccountNumber { get; set; }

        public decimal Amount { get; set; }
    }
}
