using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using TrueMoney.Data;
using TrueMoney.Data.Entities;
using TrueMoney.Models;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services
{
    public class UserService : IUserService
    {
        private readonly ITrueMoneyContext _context;

        public UserService(ITrueMoneyContext context)
        {
            _context = context;
        }

        // нормальный метод
        public async Task<UserDetailsViewModel> GetDetails(int currentUserId, int userId)
        {
            var dbUser = await _context.Users.FirstAsync(x => x.Id == userId);

            var result = new UserDetailsViewModel
            {
                User = new UserModel // use mapper here
                {
                    FirstName = dbUser.FirstName
                },
                IsCurrentUser = currentUserId == userId
            };

            return result;
        }

        public async Task<UserModel> GetById(int id)
        {
            var dbUser = await _context.Users.FirstAsync(x => x.Id == id);
            var userModel = new UserModel // use mapper here
            {
                FirstName = dbUser.FirstName
            };
            return userModel;
        }

        public async Task<IEnumerable<UserModel>> GetAll()
        {
            throw new NotImplementedException();
            //return await _userRepository.GetAll();
        }

        public async Task Add(UserModel entity)
        {
            throw new NotImplementedException();
            //await _userRepository.Add(entity);
        }

        public async Task<UserModel> GetByAspId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            throw new NotImplementedException();
            //return await _userRepository.GetByAspId(id);
        }

        public async Task<UserModel> GetUserByName(string name)
        {
            //return await this._userRepository.GetUserByName(name);
            throw new NotImplementedException();
        }

        // review: пример, где приходит слишком много полей, а могла бы приходить InputModel. Этого метода вообще быть не должно, есть Add
        public async Task<UserModel> Create(
            int id,
            string email,
            string firstName,
            string lastName,
            string middleName,
            string passportSerie,
            string passportNumber,
            string passportGiveOrganisation,
            DateTime passportDateOfIssuing,
            string bankAccountNumber,
            string aspUserId)
        {
            throw new NotImplementedException();
            ////var user = new User
            ////{
            ////    FirstName = firstName,
            ////    LastName = lastName,
            ////    MiddleName = middleName,
            ////    AspUserId = aspUserId,
            ////};
            ////user.Passport = new Passport
            ////{
            ////    Number = passportNumber,
            ////    Series = passportSerie,
            ////    DateOfIssuing = passportDateOfIssuing,
            ////    GiveOrganisation = passportGiveOrganisation
            ////};
            ////user.AccountNumber = bankAccountNumber;

            ////await _userRepository.Add(user);

            ////return user;
        }
    }
}
