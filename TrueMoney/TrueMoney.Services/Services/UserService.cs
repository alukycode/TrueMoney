using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using TrueMoney.Data;
using TrueMoney.Data.Entities;
using TrueMoney.Models;
using TrueMoney.Models.Account;
using TrueMoney.Models.Basic;

namespace TrueMoney.Services.Services
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

        public async Task Add(RegisterViewModel entity)
        {
            var user = Mapper.Map<User>(entity);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetUserIdByAspId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return (await _context.Users.FirstAsync(x => x.AspUserId == id)).Id;
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
    }
}
