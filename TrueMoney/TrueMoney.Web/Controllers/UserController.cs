using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrueMoney.Infrastructure.Services;

namespace TrueMoney.Web.Controllers
{
    using TrueMoney.Infrastructure.Entities;

    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ActionResult> Index()
        {
            var currentUser = (await _userService.GetAll()).First();
            if (currentUser == null)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.RedirectToAction("Details", new { id = currentUser.Id });
        }

        public async Task<ActionResult> Details(int id)
        {
            User user = await _userService.GetUserById(id); ;

            if (user == null)
            {
                return this.RedirectToAction("Index");
            }

            this.ViewBag.CurrentUser = user;
            return this.View(user);
        }
    }
}