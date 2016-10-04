using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueMoney.Infrastructure.Services;

namespace TrueMoney.Web.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        public string Index()
        {
            var serviceText = _userProfileService.GetShit();

            return "Controller " + serviceText;
        }
    }
}