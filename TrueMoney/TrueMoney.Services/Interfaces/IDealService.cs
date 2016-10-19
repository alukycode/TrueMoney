using System.Collections.Generic;
using System.Threading.Tasks;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services
{
    public interface IDealService
    {
        Task ApplyOffer(int offerId, int dealId);
        Task<int> CreateDeal(User user, decimal count, int rate, string description);
        Task CreateOffer(User user, int dealId, int rate);
        Task DeleteDeal(int dealId);
        Task<DealModel> FinishDealStartLoan(int userId, int offerId, int dealId);
        Task<IList<DealModel>> GetAll();
        Task<IList<DealModel>> GetAllByUser(int userId);
        Task<IList<OfferModel>> GetAllOffersByUser(int userId);
        Task<IList<DealModel>> GetAllOpen();
        Task<Deal> GetById(int id);
        Task<Deal> GetByOfferId(int offerId);
        Task<Deal> PaymentFinished(Deal deal);
        Task RevertOffer(int offerId);
    }
}