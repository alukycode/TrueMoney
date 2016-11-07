using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueMoney.Models;
using TrueMoney.Models.Account;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services.Interfaces
{
    using TrueMoney.Models.User;

    public interface IUserService
    {
        Task<UserDetailsViewModel> GetDetails(int userId);

        Task Add(RegisterViewModel entity);

        Task<IEnumerable<UserModel>> GetAll();

        Task<UserModel> GetById(int id);

        Task<UserModel> GetUserByName(string name);

        Task<AdminListViewModel> GetAdminListModel();

        Task ActivateUser(int userId);
    }
}