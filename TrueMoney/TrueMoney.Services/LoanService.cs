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

        public async Task<Loan> Create(MoneyApplication moneyApplication, Offer offer)
        {
            // add pay calcutions
            data.Add(
                new Loan
                    {
                        Borrower = moneyApplication.Borrower,
                        CloseDate = DateTime.Now,
                        Id = number++,
                        Lender = offer.Lender,
                        MoneyApplication = moneyApplication
                    });

            return data[0];
        }

        public async Task<Loan> GetById(int id)
        {
            return data.FirstOrDefault(x => x.Id == id);
        }

        public async Task<IList<Loan>> GetByUser(int userId)
        {
            return data.Where(x => x.Borrower.Id == userId || x.Lender.Id == userId).ToList();
        }
    }
}