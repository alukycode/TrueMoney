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

        private readonly IDealService _dealService;

        public LoanController(ILoanService loanService, IUserService userService, IDealService dealService)
        {
            _loanService = loanService;
            _userService = userService;
            _dealService = dealService;
        }
        public async Task<ActionResult> Index()
        {
            return new EmptyResult();
        }

        public async Task<ActionResult> Details(int? id)
        {
            ViewBag.CurrentUserId = CurrentUser.Id;
            if (id.HasValue)
            {
                var model = await _loanService.GetById(id.Value);
                if (model != null)
                {
                    return View(model);
                }
            }

            return GoHome();
        }

        [Authorize]
        public async Task<ActionResult> YouActivity()
        {
            ViewBag.IsActive = CurrentUser.IsActive;
            ViewBag.IsHaveOpenDealOrLoan = CurrentUser.IsHaveOpenDealOrLoan;
            var viewModel =  new YouActivityViewModel
                                {
                                    Deals = await _dealService.GetAllByUser(CurrentUser),
                                    Loans = await _loanService.GetAllByUser(CurrentUser),
                                    Offers = await _dealService.GetAllOffersByUser(CurrentUser)
                                };


            return View(viewModel);
        }
    }
}