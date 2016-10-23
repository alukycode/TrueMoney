using TrueMoney.Common.Enums;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services.Services
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using Bank.BankApi;
    using Bank.BankEntities;
    using Data;
    using TrueMoney.Models;
    using TrueMoney.Services.Interfaces;

    public class PaymentService : IPaymentService
    {
        private readonly IUserService _userService;
        private readonly IDealService _dealService;
        private readonly IBankApi _bankApi;
        private readonly ITrueMoneyContext _context;

        public PaymentService(IUserService userService, IDealService dealService, IBankApi bankApi, ITrueMoneyContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _userService = userService;
            _dealService = dealService;
            _bankApi = bankApi;
            _context = context;
        }
        public async Task<PaymentResult> LendMoney(VisaPaymentViewModel visaPaymentViewModel, int currentUserId)
        {
            var deal = await _context.Deals.FirstAsync(x => x.Id == visaPaymentViewModel.DealId); //тут еще, возможно, нужны какие-то проверки с текущим юзером, но если не нужны, то не добавляйте!!!
            var recipient = deal.Offers.First(x => x.IsApproved).Offerer;
            var sender = await _context.Users.FirstAsync(x => x.Id == currentUserId);

            var result = await
                _bankApi.Do(
                    new BankTransaction
                    {
                        Amount = visaPaymentViewModel.PaymentCount,
                        SenderAccountNumber = sender.BankAccountNumber,
                        RecipientAccountNumber = recipient.BankAccountNumber,
                    });

            switch (result)
            {
                case BankResponse.Success:
                    deal.DealStatus = DealStatus.InProgress;
                    await _context.SaveChangesAsync();
                    return PaymentResult.Success;

                case BankResponse.NotEnoughtMoney:
                    return PaymentResult.NotEnoughtMoney;

                default:
                    return PaymentResult.Error;
            }
        }
    }
}