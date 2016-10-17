namespace TrueMoney.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TrueMoney.Infrastructure.Entities;

    public interface IDealService
    {
        Task<IList<Deal>> GetAll();
        Task<Deal> GetById(int id);
        Task<IList<Deal>> GetAllByUser(User user);
        Task<Deal> GetByOfferId(int offerId);

        Task<IList<Offer>> GetAllOffersByUser(User user);

        Task<bool> ApplyOffer(User user, int offerId, int dealId);
        Task<bool> RevertOffer(User user, int offerId, int dealId);
        Task<bool> CreateOffer(User user, int dealId, float rate);

        Task<Deal> FinishDealStartLoan(User user, int offerId, int dealId);

        Task<int> CreateDeal(User user, float count, int paymentCount, float rate, int dayCount, string description);

        Task<bool> DeleteDeal(User user, int dealId);

        Task<Deal> PaymentFinished(Deal deal);
    }
}