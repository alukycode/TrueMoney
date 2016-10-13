namespace TrueMoney.Infrastructure.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TrueMoney.Infrastructure.Entities;

    public interface IApplicationService
    {
        Task<IList<MoneyApplication>> GetAll();
        Task<MoneyApplication> GetById(int id);
        Task<IList<MoneyApplication>> GetByUserId(int userId);
        Task<MoneyApplication> GetByOfferId(int offerId);

        Task<IList<Offer>> GetAllOffersByUser(int userId);

        Task<bool> ApplyOffer(int offerId, int moneyApplicationId);
        Task<bool> RevertOffer(int offerId, int moneyApplicationId);
        Task<bool> CreateOffer(User user, int appId, float rate);

        Task<int> FinishApp(User user, int offerId, int moneyApplicationId);

        Task<int> CreateApp(User user, float count, int paymentCount, float rate, int dayCount, string description);

        Task<bool> DeleteApp(User user, int appId);
    }
}