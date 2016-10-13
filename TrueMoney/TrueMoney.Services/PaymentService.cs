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

        private readonly ILoanService _loanService;

        private readonly IBankApi _bankApi;

        public PaymentService(IUserService userService, ILoanService loanService, IBankApi bankApi)
        {
            _userService = userService;
            _loanService = loanService;
        }
        public async Task<PaymentResult> LendMoney(int loanId, int payForId, float count, VisaDetails visaDetails)
        {
            var user = await _userService.GetCurrentUser();
            var payForUser = await _userService.GetUserById(payForId);
            var loan = await _loanService.GetById(loanId);

            if (user != null && payForUser != null && loan != null && Equals(loan.Borrower, payForUser) && Equals(loan.Lender, user))
            {
                var result =
                    _bankApi.Do(
                        new Bank.BankEntities.BankTransaction
                        {
                            Amount = count,
                            AccountNumber1 = user.BankAccount.AccountNumber,
                            AccountNumber2 = payForUser.BankAccount.AccountNumber,
                            BankAction = BankAction.Transfer,
                            Secret = ""
                        });

                switch (result)
                {
                    case BankResponse.Success:
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