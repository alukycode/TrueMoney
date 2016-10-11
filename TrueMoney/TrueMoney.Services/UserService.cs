using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueMoney.Infrastructure.Entities;
using TrueMoney.Infrastructure.Repositories;
using TrueMoney.Infrastructure.Services;

namespace TrueMoney.Services
{
    using System.Web;

    public class UserService: IUserService
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

        public async Task<User> GetCurrentUser()
        {
            int userId;
            if(Int32.TryParse(HttpContext.Current.Request.QueryString["user"], out userId))
            {
                return new User { Id = userId, IsActive = true };
            }

            return new User { Id = 1, IsActive = true };
        }

        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetById(id);
        }

        public async Task<User> GetUserByName(string name)
        {
            //return await this._userRepository.GetUserByName(name);
            throw new NotImplementedException();
        }

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
            user.Passport.Number = passportNumber;
            user.Passport.Series = passportSerie;
            user.Passport.DateOfIssuing = passportDateOfIssuing;
            user.Passport.GiveOrganisation = passportGiveOrganisation;

            await _userRepository.Add(user);

            return user;
        }
    }
}
