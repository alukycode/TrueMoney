using System.Collections.Generic;
using System.Threading.Tasks;

namespace TrueMoney.Infrastructure.Repositories
{
    public interface IRepository<T>
    {
        Task<T> GetById(int id);

        Task<IList<T>> GetAll();
    }
}
