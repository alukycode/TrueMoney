using System.Collections.Generic;
using System.Threading.Tasks;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services.Interfaces
{
    using Models.Offer;
    using TrueMoney.Models;
    using TrueMoney.Models.Deal;
    using TrueMoney.Models.User;

    public interface IDealService
    {
        Task<int> CreateDeal(CreateDealForm createDealForm, int userId);
        
        Task DeleteDeal(int dealId, int userId);

        Task FinishDealStartLoan(int offerId);

        Task<DealIndexViewModel> GetAll(int currentUserId);

        Task<IList<DealModel>> GetByUser(int userId);

        Task<DealDetailsViewModel> GetById(int id, int userId);

        //Task<Deal> GetByOfferId(int offerId); пока не используется, ну и сервисы не должны возвращать сущности базы

        Task<CreateDealForm> GetCreateDealForm(int userId);
    }
}