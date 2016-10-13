namespace TrueMoney.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TrueMoney.Infrastructure.Entities;

    public interface ILoanService
    {
        Task<Loan> Create(User user, MoneyApplication moneyApplication, Offer offer);
        Task<Loan> GetById(int id, int userId); // review: так что мы будем юзать, юзера или userId?
        Task<IList<Loan>> GetByUser(int userId);

        Task<bool> StartLoan(Loan loan);
    }
}