using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueMoney.Data.Entities;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services
{
    using System.Web;

    public class UserService : IUserService
    {
        public UserService()
        {
            
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

        public async Task<UserModel> GetById(int id)
        {
            throw new NotImplementedException();
            //return await _userRepository.GetById(id);
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
