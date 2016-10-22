using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TrueMoney.Services;

namespace TrueMoney.Web.Controllers
{
    public class UserController : BaseController
    {
        public UserController(IUserService userService) : base(userService)
        {
        }

        public async Task<ActionResult> Index()
        {
            return RedirectToAction("Details", new { id = CurrentUserId });
        }


        // нормальный метод
        public async Task<ActionResult> Details(int id)
        {
            var userModel = await _userService.GetDetails(CurrentUserId, id);

            return View(userModel);
        }
    }
}