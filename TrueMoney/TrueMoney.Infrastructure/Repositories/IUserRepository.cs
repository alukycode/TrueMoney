using TrueMoney.Infrastructure.Entities;

namespace TrueMoney.Infrastructure.Repositories
{
    using System.Threading.Tasks;

    public interface IUserRepository : IRepository<User>
    {
        Task Add(User entity);

        Task Update(User entity);

        Task Delete(int id);
    }
}
