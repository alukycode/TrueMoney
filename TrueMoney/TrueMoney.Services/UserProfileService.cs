namespace TrueMoney.Services
{
    using TrueMoney.Infrastructure.Repositories;
    using TrueMoney.Infrastructure.Services;

    public class UserProfileService: IUserProfileService
    {
        private readonly IUserRepository _userProfileRepository;

        public UserProfileService(IUserRepository userProfileRepository)
        {
            this._userProfileRepository = userProfileRepository;
        }

        public string GetShit()
        {
            var repoText = this._userProfileRepository.GetShit();

            return "Service " + repoText;
        }
    }
}
