namespace TrueMoney.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TrueMoney.Infrastructure.Entities;

    public interface ILoanService
    {
        Task<Loan> Create(User user, Deal deal, Offer offer);
        Task<Loan> GetById(int id); // review: так что мы будем юзать, юзера или userId? юзера, у него же есть просто ид, а есть асп ид, хз что убодней
        Task<IList<Loan>> GetAllByUser(User user);

        Task<bool> StartLoan(Loan loan);
    }
}