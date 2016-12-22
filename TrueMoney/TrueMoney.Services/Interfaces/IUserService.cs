using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueMoney.Data.Entities;
using TrueMoney.Models;
using TrueMoney.Models.Account;
using TrueMoney.Models.Admin;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services.Interfaces
{
    using TrueMoney.Models.User;

    public interface IUserService
    {
        Task<UserDetailsViewModel> GetDetails(int userId);

        User GetMappedUserEnity(RegisterViewModel model);

        Task<IEnumerable<UserModel>> GetAll();

        Task<UserModel> GetById(int id);

        Task<InactiveUsersViewModel> GetInactiveUsersViewModel();

        //Task<UserActivityViewModel> GetProfileViewModel(int currentUserId);

        Task ActivateUser(int userId);

        Task DeactivateUser(int userId);

        Task<EditUserViewModel> GetEditModel(int id);

        Task<EditPassportViewModel> GetEditPassportModel(int userId);

        Task Update(EditUserViewModel model);

        Task UpdatePassport(EditPassportViewModel model);

        Task<UserActivityViewModel> GetUserActivityModel(int userId);

        Task<UserProfileModel> GetUserProfileModel(int userId);
    }
}