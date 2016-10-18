using TrueMoney.Infrastructure.Enums;

namespace TrueMoney.Infrastructure.Services
{
    using System.Threading.Tasks;

    using TrueMoney.Infrastructure.Entities;

    public interface IPaymentService
    {
        Task<PaymentResult> LendMoney(User user, int dealId, decimal amount, VisaDetails visaDetails);
        //Task<PaymentResult> PayLoanPart(int dealId, int payerId, float count);
    }
}