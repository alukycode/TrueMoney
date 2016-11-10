using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrueMoney.Common;
using TrueMoney.Services.Interfaces;

namespace TrueMoney.Web.Controllers
{
    [Authorize(Roles = RoleNames.Admin)]
    public class AdminController : BaseController
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ActionResult> InactiveUsers()
        {
            var model = await _userService.GetInactiveUsersViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Activate(int userId)
        {
            await _userService.ActivateUser(userId);
            return RedirectToAction("InactiveUsers");
        }
    }
}