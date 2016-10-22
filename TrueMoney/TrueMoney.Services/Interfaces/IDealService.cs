using System.Collections.Generic;
using System.Threading.Tasks;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services
{
    using TrueMoney.Models.ViewModels;

    public interface IDealService
    {
        Task ApplyOffer(int offerId, int dealId);
        Task<int> CreateDeal(User user, decimal count, int rate, string description);
        Task CreateOffer(User user, int dealId, int rate);
        Task DeleteDeal(int dealId);
        Task<DealModel> FinishDealStartLoan(int userId, int offerId, int dealId);
        Task<IList<DealIndexViewModel>> GetAll(int userId);
        Task<IList<DealModel>> GetAllByUser(int userId);
        Task<IList<OfferModel>> GetAllOffersByUser(int userId);
        Task<IList<DealIndexViewModel>> GetAllOpen(int userId);
        Task<DealDetailsViewModel> GetById(int id, int userId);
        Task<Deal> GetByOfferId(int offerId);
        Task<DealModel> PaymentFinished(DealModel deal);
        Task RevertOffer(int offerId);
    }
}