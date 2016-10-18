using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueMoney.Infrastructure.Entities;
using TrueMoney.Infrastructure.Repositories;
using TrueMoney.Infrastructure.Services;

namespace TrueMoney.Services
{
    using System.Web;

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userRepository.GetAll();
        }

        public async Task Add(User entity)
        {
            await _userRepository.Add(entity);
        }

        public async Task<User> GetById(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<User> GetByAspId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _userRepository.GetByAspId(id);
        }

        public async Task<User> GetUserByName(string name)
        {
            //return await this._userRepository.GetUserByName(name);
            throw new NotImplementedException();
        }

        // review: пример, где приходит слишком много полей, а могла бы приходить InputModel. Этого метода вообще быть не должно, есть Add
        public async Task<User> Create(
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
            var user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                MiddleName = middleName,
                AspUserId = aspUserId,
            };
            user.Passport = new Passport
            {
                Number = passportNumber,
                Series = passportSerie,
                DateOfIssuing = passportDateOfIssuing,
                GiveOrganisation = passportGiveOrganisation
            };
            user.AccountNumber = bankAccountNumber;

            await _userRepository.Add(user);

            return user;
        }
    }
}
