using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TrueMoney.Services;

namespace TrueMoney.Web.Controllers
{
    public class UserController : BaseController
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
                return RedirectToAction("Index", "Home");
            }

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