using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueMoney.Infrastructure.Entities;

namespace TrueMoney.Infrastructure.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAll();

        Task Add(User entity);

        Task<User> GetCurrentUser();
        Task<User> GetUserById(int id);
        Task<User> GetUserByName(string name);
    }
}
