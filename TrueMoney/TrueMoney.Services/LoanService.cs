namespace TrueMoney.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using TrueMoney.Infrastructure.Entities;
    using TrueMoney.Infrastructure.Services;

    public class LoanService : ILoanService
    {
        static List<Loan> data = new List<Loan>();

        private static int number = 0;

        private readonly IUserService _userService;

        public LoanService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Loan> Create(MoneyApplication moneyApplication, Offer offer)
        {
            var currentUser = await _userService.GetCurrentUser();
            if (currentUser != null && currentUser.IsActive && !currentUser.IsHaveOpenAppOrLoan && 
                moneyApplication.IsTakePart(currentUser) && Equals(offer.Lender, currentUser))
            {
                // add pay calcutions
                data.Add(
                    new Loan
                    {
                        Borrower = moneyApplication.Borrower,
                        CloseDate = DateTime.Now,
                        Id = number++,
                        Lender = offer.Lender,
                        MoneyApplication = moneyApplication,
                        Rate = offer.Rate,
                        Count = moneyApplication.Count
                    });

                return data[0];
            }

            return null;
        }

        public async Task<Loan> GetById(int id)
        {
            return data.FirstOrDefault(x => x.Id == id);
        }

        public async Task<IList<Loan>> GetByUser(int userId)
        {
            return data.Where(x => x.Borrower.Id == userId || x.Lender.Id == userId).ToList();
        }

        public async Task<bool> CtartLoan(Loan loan)
        {
            loan.IsWaitForMoney = false;
            loan.IsStarted = true;
            loan.Payments = new List<Payment>();//todo generate

            return true;
        } 
    }
}