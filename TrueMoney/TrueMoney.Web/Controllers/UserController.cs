using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TrueMoney.Common;
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

        [Authorize]
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
        public async Task<ActionResult> UserProfile() // имя Profile уже занято в контроллере :(
        {
            var viewModel = await _userService.GetProfileViewModel(User.Identity.GetUserId<int>());

            return View(viewModel);
        }
    }
}