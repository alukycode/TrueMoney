namespace TrueMoney.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TrueMoney.Infrastructure.Entities;

    public interface ILoanService
    {
        Task<Loan> Create(MoneyApplication moneyApplication, Offer offer);
        Task<Loan> GetById(int id);
        Task<IList<Loan>> GetByUser(int userId);
    }
}