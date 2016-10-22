using TrueMoney.Common.Enums;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Bank.BankApi;
    using Bank.BankEntities;

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
        public async Task<PaymentResult> LendMoney(int userId, int dealId, decimal amount) // todo: VisaDetails lost after structure changes, create model
        {
            var deal = await _dealService.GetById(dealId, userId);
            var recipientId = deal.Deal.Offers.First(x => x.IsApproved).LenderId;
            var recipient = await _userService.GetById(recipientId);

            var result = await
                _bankApi.Do(
                    new Bank.BankEntities.BankTransaction
                    {
                        Amount = amount,
                        // todo: commented after structure changes
                        //SenderAccountNumber = user.AccountNumber,
                        //RecipientAccountNumber = recipient.AccountNumber,
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