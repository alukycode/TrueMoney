using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TrueMoney.Models.Basic;
using TrueMoney.Services;
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

        // todo: async await?
        public int CurrentUserId
        {
            get
            {
                var currentUserIdentityId = User.Identity.GetUserId();
                if (currentUserIdentityId == null)
                {
                    throw new UnauthorizedAccessException("user should be authorized to perform this action");
                }

                var id = _userService.GetUserIdByAspId(currentUserIdentityId);
                return id.Result;
            }
        }

        protected ActionResult GoHome() // Сомнительный метод
        {
            return RedirectToAction("YouActivity", "Deal");
        }
    }
}