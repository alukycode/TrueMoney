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

        public async Task<Loan> Create(User currentUser, Deal deal, Offer offer)
        {
            if (currentUser != null && currentUser.IsActive && !currentUser.IsHaveOpenDealOrLoan && 
                deal.IsTakePart(currentUser) && Equals(offer.Lender, currentUser))
            {
                // add pay calcutions
                data.Add(
                    new Loan
                    {
                        Borrower = deal.Borrower,
                        CloseDate = DateTime.Now,
                        Id = number++,
                        Lender = offer.Lender,
                        Deal = deal,
                        Rate = offer.Rate,
                        Count = deal.Count
                    });

                return data[0];
            }

            return null;
        }

        public async Task<Loan> GetById(int id, int userId)
        {
            // todo: add user validation here and throw exception if he isn't valid
            return data.FirstOrDefault(x => x.Id == id);
        }

        public async Task<IList<Loan>> GetByUser(int userId)
        {
            return data.Where(x => x.Borrower.Id == userId || x.Lender.Id == userId).ToList();
        }

        public async Task<bool> StartLoan(Loan loan)
        {
            loan.IsWaitForMoney = false;
            loan.IsStarted = true;
            loan.Payments = new List<Payment>();//todo generate

            return true;
        } 
    }
}