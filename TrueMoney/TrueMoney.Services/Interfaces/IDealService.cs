using System.Collections.Generic;
using System.Threading.Tasks;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services.Interfaces
{
    using TrueMoney.Models;
    using TrueMoney.Models.ViewModels;

    public interface IDealService
    {
        Task ApproveOffer(int offerId);

        Task<int> CreateDeal(CreateDealForm createDealForm, int userId);

        Task CreateOffer(CreateOfferForm createOfferForm, int userId);

        Task DeleteDeal(int dealId, int userId);

        Task<DealModel> FinishDealStartLoan(int userId, int offerId, int dealId);

        Task<DealIndexViewModel> GetAll(int userId);

        Task<IList<DealModel>> GetByUser(int userId);

        Task<DealIndexViewModel> GetAllOpen(int userId);

        Task<DealDetailsViewModel> GetById(int id, int userId);

        //Task<Deal> GetByOfferId(int offerId); пока не используется, ну и сервисы не должны возвращать сущности базы

        Task<DealModel> PaymentFinished(DealModel deal);

        Task RevertOffer(int offerId);

        Task<CreateDealForm> GetCreateDealForm(int userId);

        Task<CreateOfferForm> GetCreateOfferForm(int dealId, int userId);

        Task<YourActivityViewModel> GetYourActivityViewModel(int currentUserId);
    }
}