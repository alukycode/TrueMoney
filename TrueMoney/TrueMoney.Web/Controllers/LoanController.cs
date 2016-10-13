using System.Web.Mvc;

namespace TrueMoney.Web.Controllers
{
    using System.Threading.Tasks;

    using Infrastructure.Services;

    [Authorize]
    public class LoanController : Controller
    {
        private readonly ILoanService _loanService;
        private readonly IUserService _userService;

        public LoanController(ILoanService loanService, IUserService userService)
        {
            _loanService = loanService;
            _userService = userService;
        }
        public async Task<ActionResult> Index()
        {
            return new EmptyResult();
        }

        public async Task<ActionResult> Details(int id)
        {
            var currentUser = await _userService.GetCurrentUser();
            ViewBag.CurrentUser = currentUser;
            var model = await _loanService.GetById(id);
            if (model.IsTakePart(currentUser))
            {
                return View(model);
            }

            return RedirectToAction("Index");
        }

        public string Test()
        {
            return "authorized";
        }
    }
}