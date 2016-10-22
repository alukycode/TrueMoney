using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueMoney.Models;
using TrueMoney.Models.Account;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services
{
    public interface IUserService
    {
        Task<UserDetailsViewModel> GetDetails(int currentUserId, int userId);

        Task Add(RegisterViewModel entity);

        Task<IEnumerable<UserModel>> GetAll();

        Task<UserModel> GetByAspId(string id);

        Task<UserModel> GetById(int id);

        Task<UserModel> GetUserByName(string name);
    }
}