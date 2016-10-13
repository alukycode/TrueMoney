using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace TrueMoney.Web.Controllers
{
    using System.Threading.Tasks;

    using Infrastructure.Services;

    using TrueMoney.Web.Models;

    [Authorize]
    public class LoanController : BaseController
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
            var model = await _loanService.GetById(id, CurrentUser.Id);

            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> YouActivity()
        {
            var viewModel =  new YouActivityViewModel
                                {
                                    MoneyApplications = await _applicationService.GetByUserId(CurrentUser.Id),
                                    Loans = await _loanService.GetByUser(CurrentUser.Id),
                                    Offers = await _applicationService.GetAllOffersByUser(CurrentUser.Id)
                                };


            return View(viewModel);
        }
    }
}