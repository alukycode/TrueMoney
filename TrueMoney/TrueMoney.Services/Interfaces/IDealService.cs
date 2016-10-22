using System.Collections.Generic;
using System.Threading.Tasks;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services
{
    using TrueMoney.Models;
    using TrueMoney.Models.ViewModels;

    public interface IDealService
    {
        Task ApplyOffer(int offerId, int dealId);
        Task<DealModel> CreateDeal(CreateDealForm createDealForm, int userId);
        Task<DealDetailsViewModel> CreateOffer(CreateOfferForm createOfferForm, int userId);
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
        Task<CreateDealForm> GetCreateDealForm(int userId);
        Task<CreateOfferForm> GetCreateOfferForm(int dealId, int userId);
    }
}