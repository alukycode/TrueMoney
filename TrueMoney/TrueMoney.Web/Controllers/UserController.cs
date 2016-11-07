using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TrueMoney.Common;
using TrueMoney.Services.Interfaces;

namespace TrueMoney.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Details", new { id = CurrentUserId });
        }

        public async Task<ActionResult> Details(int id)
        {
            var userModel = await _userService.GetDetails(CurrentUserId, id);

            return View(userModel);
        }

        [Authorize(Roles = RoleNames.Admin)]
        public async Task<ActionResult> AdminList()
        {
            return View(await _userService.GetAdminListModel());
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost]
        public async Task<ActionResult> Activate(int userId)
        {
            await _userService.ActivateUser(userId);
            return RedirectToAction("AdminList");
        }
    }
}