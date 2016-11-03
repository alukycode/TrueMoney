using System.Collections.Generic;
using System.Threading.Tasks;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services.Interfaces
{
    using TrueMoney.Models;
    using TrueMoney.Models.Deal;
    using TrueMoney.Models.User;
    using TrueMoney.Models.ViewModels;

    public interface IDealService
    {
        Task ApproveOffer(int offerId);

        Task CancelOfferApproval(int offerId);

        Task<int> CreateDeal(CreateDealForm createDealForm, int userId);

        Task CreateOffer(CreateOfferForm createOfferForm, int userId);

        Task DeleteDeal(int dealId, int userId);

        Task FinishDealStartLoan(int userId, int offerId, int dealId);

        Task<DealIndexViewModel> GetAll(int currentUserId);

        Task<IList<DealModel>> GetByUser(int userId);

        Task<DealIndexViewModel> GetAllOpen(int userId);

        Task<DealDetailsViewModel> GetById(int id, int userId);

        //Task<Deal> GetByOfferId(int offerId); пока не используется, ну и сервисы не должны возвращать сущности базы
        
        Task RevertOffer(int offerId);

        Task<CreateDealForm> GetCreateDealForm(int userId);

        Task<CreateOfferForm> GetCreateOfferForm(int dealId, int userId);

        Task<YourActivityViewModel> GetYourActivityViewModel(int currentUserId);
    }
}