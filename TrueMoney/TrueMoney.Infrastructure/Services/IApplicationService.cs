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

        Task<bool> ApplyOffer(int offerId, int moneyApplicationId);
        Task<bool> RevertOffer(int offerId, int moneyApplicationId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="offerId"></param>
        /// <param name="moneyApplicationId"></param>
        /// <returns>New Loan Id</returns>
        Task<int> FinishApp(int offerId, int moneyApplicationId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>New money application id</returns>
        Task<int> CreateApp(float count, float rate, int dayCount, string description);
    }
}