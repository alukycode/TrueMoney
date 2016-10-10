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

        public async Task<User> GetCurrentUser()
        {
            int userId;
            if(Int32.TryParse(HttpContext.Current.Request.QueryString["user"], out userId))
            {
                return new User { Id = userId };
            }

            return new User { Id = 1 };
        }
    }
}
