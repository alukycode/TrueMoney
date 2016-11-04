namespace TrueMoney.Services.Interfaces
{
    using System.Threading.Tasks;

    using TrueMoney.Common.Enums;
    using TrueMoney.Models;

    public interface IPaymentService
    {
        Task<PaymentResult> LendMoney(VisaPaymentViewModel visaPaymentViewModel, int userId);

        Task<PaymentResult> Payout(VisaPaymentViewModel visaPaymentViewModel, int currentUserId);
    }
}