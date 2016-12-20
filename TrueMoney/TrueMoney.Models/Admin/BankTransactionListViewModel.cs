namespace TrueMoney.Models.Admin
{
    using System.Collections.Generic;

    using TrueMoney.Models.Basic;

    public class BankTransactionListViewModel
    {
        public IList<BankTransactionModel> Transactions { get; set; } 
    }
}