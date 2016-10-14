namespace TrueMoney.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TrueMoney.Infrastructure.Entities;

    public interface IDealService
    {
        Task<IList<Deal>> GetAll();
        Task<Deal> GetById(int id);
        Task<IList<Deal>> GetByUserId(int userId);
        Task<Deal> GetByOfferId(int offerId);

        Task<IList<Offer>> GetAllOffersByUser(int userId);

        Task<bool> ApplyOffer(int offerId, int dealId);
        Task<bool> RevertOffer(int offerId, int dealId);
        Task<bool> CreateOffer(User user, int dealId, float rate);

        Task<int> FinishDeal(User user, int offerId, int dealId);

        Task<int> CreateDeal(User user, float count, int paymentCount, float rate, int dayCount, string description);

        Task<bool> DeleteDeal(User user, int dealIp);
    }
}