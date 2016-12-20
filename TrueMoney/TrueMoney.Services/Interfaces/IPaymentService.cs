namespace TrueMoney.Services.Interfaces
{
    using System.Threading.Tasks;

    using TrueMoney.Common.Enums;
    using TrueMoney.Models;
    using TrueMoney.Models.Admin;

    public interface IPaymentService
    {
        Task<PaymentResult> LendMoney(VisaPaymentViewModel visaPaymentViewModel, int userId);

        Task<PaymentResult> Payout(VisaPaymentViewModel visaPaymentViewModel);

        Task<BankTransactionListViewModel> GetBankTransactions();
    }
}