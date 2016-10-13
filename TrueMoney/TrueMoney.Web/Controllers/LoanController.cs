using System.Web.Mvc;

namespace TrueMoney.Web.Controllers
{
    using System.Threading.Tasks;

    using Infrastructure.Services;

    using TrueMoney.Web.Models;

    [Authorize]
    public class LoanController : Controller
    {
        private readonly ILoanService _loanService;
        private readonly IUserService _userService;

        private readonly IApplicationService _applicationService;

        public LoanController(ILoanService loanService, IUserService userService, IApplicationService applicationService)
        {
            _loanService = loanService;
            _userService = userService;
            _applicationService = applicationService;
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

        [Authorize]
        public async Task<ActionResult> YouActivity()
        {
            var currentUser = await _userService.GetCurrentUser();
            ViewBag.CurrentUser = currentUser;
            var viewModel = new YouActivityViewModel
                                {
                                    MoneyApplications =
                                        await _applicationService.GetByUserId(currentUser.Id),
                                    Loans = await _loanService.GetByUser(currentUser.Id),
                                    Offers =
                                        await
                                        _applicationService.GetAllOffersByUser(currentUser.Id)
                                };
            return View(viewModel);
        }
    }
}