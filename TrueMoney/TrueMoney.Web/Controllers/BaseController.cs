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
        ////private readonly IUserService _userService;

        public BaseController()
        {
            ////_userService = DependencyResolver.Current.GetService<IUserService>();
        }

        public int CurrentUserId
        {
            get
            {
                return User.Identity.GetUserId<int>(); 
            }
        }

        protected ActionResult GoHome() // Сомнительный метод, удалять его я, конечно же, не буду.
        {
            return RedirectToAction("YourActivity", "Deal");
        }
    }
}