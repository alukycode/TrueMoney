using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueMoney.Infrastructure.Repositories;
using TrueMoney.Infrastructure.Services;

namespace TrueMoney.Services
{
    public class UserProfileService: IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }

        public string GetShit()
        {
            var repoText = _userProfileRepository.GetShit();

            return "Service " + repoText;
        }
    }
}
