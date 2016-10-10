using TrueMoney.Infrastructure.Entities;

namespace TrueMoney.Infrastructure.Repositories
{
    using System.Threading.Tasks;

    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Search in first name, last name and family name fields for coinsitence
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<User> GetUserByName(string name);
    }
}
