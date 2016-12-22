using TrueMoney.Models.Basic;

namespace TrueMoney.Models.Admin
{
    public class TransactionAdminModel
    {
        public BankTransactionModel Transaction { get; set; }

        public UserModel From { get; set; }

        public UserModel To { get; set; }
    }
}