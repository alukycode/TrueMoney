using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TrueMoney.Common;
using TrueMoney.Models.User;
using TrueMoney.Services.Interfaces;

namespace TrueMoney.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Details", new { id = User.Identity.GetUserId<int>() }); 
        }

        public async Task<ActionResult> Details(int id)
        {
            var userModel = await _userService.GetDetails(id);

            return View(userModel);
        }

        [Authorize(Roles = RoleNames.User)]
        public async Task<ActionResult> UserProfile()
        {
            var model = await _userService.GetUserProfileModel(User.Identity.GetUserId<int>());

            return View(model);
        }

        public async Task<ActionResult> Edit()
        {
            var editModel = await _userService.GetEditModel(User.Identity.GetUserId<int>());

            return View(editModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EditUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _userService.Update(model);

                return RedirectToAction("UserProfile");
            }

            return View(model);
        }

        public async Task<ActionResult> EditPassport()
        {
            var editModel = await _userService.GetEditPassportModel(User.Identity.GetUserId<int>());

            return View(editModel);
        }

        [HttpPost]
        public async Task<ActionResult> EditPassport(EditPassportViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _userService.UpdatePassport(model);

                return RedirectToAction("UserProfile");
            }

            return View(model);
        }

        public async Task<ActionResult> UserActivity()
        {
            var model = await _userService.GetUserActivityModel(User.Identity.GetUserId<int>());

            return View(model);
        }
    }
}