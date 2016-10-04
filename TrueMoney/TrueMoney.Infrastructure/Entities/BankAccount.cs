namespace TrueMoney.Infrastructure.Entities
{
    using System.Collections.Generic;

    public class BankAccount : BaseEntity
    {
        public string AccountNumber { get; set; }

        public User Owner { get; set; }

        /// <summary>
        /// All transactions connected with that bank account
        /// </summary>
        public IList<BankTransaction> Transactions { get; set; }
    }
}
