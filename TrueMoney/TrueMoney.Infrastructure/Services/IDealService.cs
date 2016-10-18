namespace TrueMoney.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TrueMoney.Infrastructure.Entities;

    public interface IDealService
    {
        Task<IList<Deal>> GetAllOpen();
        Task<Deal> GetById(int id);
        Task<IList<Deal>> GetAllByUser(User user);
        Task<Deal> GetByOfferId(int offerId);

        Task<IList<Offer>> GetAllOffersByUser(User user);

        Task ApplyOffer(int offerId, int dealId);
        Task RevertOffer(int offerId);
        Task CreateOffer(User user, int dealId, int rate);

        Task<Deal> FinishDealStartLoan(User user, int offerId, int dealId);

        Task<int> CreateDeal(User user, decimal count, int rate, string description);

        Task DeleteDeal(int dealId);

        Task<Deal> PaymentFinished(Deal deal);
    }
}