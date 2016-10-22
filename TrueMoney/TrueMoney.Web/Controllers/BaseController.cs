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
            _userService = DependencyResolver.Current.GetService<UserService>();
        }

        // todo: async await?
        public int CurrentUserId
        {
            get
            {
                return _userService.GetUserIdByAspId(User.Identity.GetUserId()).Result;
            }
        }

        protected ActionResult GoHome() // Сомнительный метод
        {
            return RedirectToAction("YouActivity", "Deal");
        }
    }
}