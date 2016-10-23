using TrueMoney.Common.Enums;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Bank.BankApi;
    using Bank.BankEntities;

    using TrueMoney.Models;
    using TrueMoney.Services.Interfaces;

    public class PaymentService : IPaymentService
    {
        private readonly IUserService _userService;
        private readonly IDealService _dealService;

        private readonly IBankApi _bankApi;

        public PaymentService(IUserService userService, IDealService dealService, IBankApi bankApi)
        {
            _userService = userService;
            _dealService = dealService;
            _bankApi = bankApi;
        }
        public async Task<PaymentResult> LendMoney(VisaPaymentViewModel visaPaymentViewModel, int userId)
        {
            var deal = await _dealService.GetById(visaPaymentViewModel.DealId, userId);
            var recipient = deal.CurrentUserOffer.Lender;

            var result = await
                _bankApi.Do(
                    new Bank.BankEntities.BankTransaction
                    {
                        Amount = visaPaymentViewModel.PaymentCount,
                        SenderAccountNumber = deal.Deal.Borrower.BankAccountNumber,
                        RecipientAccountNumber = recipient.BankAccountNumber,
                    });

            switch (result)
            {
                case BankResponse.Success:
                    await _dealService.PaymentFinished(deal.Deal);
                    return PaymentResult.Success;
                case BankResponse.NotEnoughtMoney:
                    return PaymentResult.NotEnoughtMoney;
                default:
                    return PaymentResult.Error;
            }
        }
    }
}