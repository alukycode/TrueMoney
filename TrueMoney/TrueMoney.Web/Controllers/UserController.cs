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

        public async Task<ActionResult> Details(int id)
        {
            var user = await _userService.GetById(id); ;

            if (user == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.CurrentUser = user;
            return View(user);
        }
    }
}