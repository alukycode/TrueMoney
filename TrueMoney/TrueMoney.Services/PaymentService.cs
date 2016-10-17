namespace TrueMoney.Services
{
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
        public async Task<PaymentResult> LendMoney(User user, int dealId, int payForId, float count, VisaDetails visaDetails)
        {
            var payForUser = await _userService.GetUserById(payForId);
            var deal = await _dealService.GetById(dealId);

            if (payForUser != null && deal != null && Equals(deal.Borrower, payForUser) && Equals(deal.Lender, user))
            {
                var result = await
                    _bankApi.Do(
                        new Bank.BankEntities.BankTransaction
                        {
                            Amount = count,
                            AccountNumber1 = user.AccountNumber,
                            AccountNumber2 = payForUser.AccountNumber,
                            BankAction = BankAction.Transfer,
                            Secret = ""
                        });

                switch (result)
                {
                    case BankResponse.Success:
                        await _dealService.PaymentFinished(deal);
                        return PaymentResult.Success;
                    case BankResponse.NotEnoughtMoney:
                        return PaymentResult.NotEnoughtMoney;
                    case BankResponse.Error:
                    case BankResponse.PermissionError:
                    case BankResponse.EmptyData:
                        return PaymentResult.Error;
                }
            }

            return PaymentResult.PermissionError;
        }
    }
}