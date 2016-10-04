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

        public BankAction BankAction { get; set; }

        public string AccountNumber1 { get; set; }

        public string AccountNumber2 { get; set; }

        public string Secret { get; set; }

        public float Amount { get; set; }
    }
}
