using TrueMoney.Infrastructure.Enums;

namespace TrueMoney.Services
{
    using System.Linq;
    using System.Threading.Tasks;

    using Bank.BankApi;
    using Bank.BankEntities;

    using TrueMoney.Infrastructure.Entities;
    using TrueMoney.Infrastructure.Services;

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
        public async Task<PaymentResult> LendMoney(User user, int dealId, decimal amount, VisaDetails visaDetails)
        {
            var deal = await _dealService.GetById(dealId);
            var recipientId = deal.Offers.First(x => x.IsApproved).Offerer.Id;
            var recipient = await _userService.GetById(recipientId);

            var result = await
                _bankApi.Do(
                    new Bank.BankEntities.BankTransaction
                    {
                        Amount = amount,
                        SenderAccountNumber = user.AccountNumber,
                        RecipientAccountNumber = recipient.AccountNumber,
                    });

            switch (result)
            {
                case BankResponse.Success:
                    await _dealService.PaymentFinished(deal);
                    return PaymentResult.Success;
                case BankResponse.NotEnoughtMoney:
                    return PaymentResult.NotEnoughtMoney;
                default:
                    return PaymentResult.Error;
            }
        }
    }
}