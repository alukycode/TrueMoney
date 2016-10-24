using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TrueMoney.Models.Basic;
using TrueMoney.Services;
using TrueMoney.Services.Interfaces;
using TrueMoney.Services.Services;

namespace TrueMoney.Web.Controllers
{
    public class BaseController : Controller
    {
        private readonly IUserService _userService;

        public BaseController()
        {
            _userService = DependencyResolver.Current.GetService<IUserService>();
        }

        public async Task<int> CurrentUserId()
        {
            var currentUserIdentityId = User.Identity.GetUserId();
            if (currentUserIdentityId == null)
            {
                throw new UnauthorizedAccessException("user should be authorized to perform this action");
            }

            return await _userService.GetUserIdByAspId(currentUserIdentityId);
        }

        protected ActionResult GoHome() // Сомнительный метод, удалять его я, конечно же, не буду.
        {
            return RedirectToAction("YourActivity", "Deal");
        }
    }
}