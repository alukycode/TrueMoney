using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TrueMoney.Infrastructure.Services;

namespace TrueMoney.Web.Controllers
{
    using Infrastructure.Entities;

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
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Details", new { id = currentUser.Id });
        }

        public async Task<ActionResult> Details(int id)
        {
            User user = await _userService.GetUserById(id); ;

            if (user == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.CurrentUser = user;
            return View(user);
        }
    }
}