using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TrueMoney.Common;
using TrueMoney.Services.Interfaces;

namespace TrueMoney.Web.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        public ActionResult Index()
        {
            return RedirectToAction("Details", new { id = CurrentUserId }); 
        }

        public async Task<ActionResult> Details(int id)
        {
            var userModel = await _userService.GetDetails(id);

            return View(userModel);
        }

        [Authorize(Roles = RoleNames.User)]
        public async Task<ActionResult> UserProfile() // имя Profile уже занято в контроллере :(
        {
            var viewModel = await _userService.GetProfileViewModel(CurrentUserId);

            return View(viewModel);
        }
    }
}