using System.Threading.Tasks;
using TrueMoney.Common.Enums;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services
{
    public interface IPaymentService
    {
        Task<PaymentResult> LendMoney(int userId, int dealId, decimal amount);
    }
}