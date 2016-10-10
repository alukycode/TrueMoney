using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrueMoney.Web.Controllers
{
    using System.Threading.Tasks;

    using TrueMoney.Infrastructure.Services;

    [Authorize]
    public class LoanController : Controller
    {
        private readonly ILoanService _loanService;
        private readonly IUserService _userService;

        public LoanController(ILoanService loanService, IUserService userService)
        {
            this._loanService = loanService;
            this._userService = userService;
        }
        public async Task<ActionResult> Index()
        {
            return new EmptyResult();
        }

        public async Task<ActionResult> Details(int id)
        {
            var currentUser = await this._userService.GetCurrentUser();
            this.ViewBag.CurrentUser = currentUser;
            var model = await this._loanService.GetById(id);
            if (model.IsTakePart(currentUser))
            {
                return this.View(model);
            }

            return this.RedirectToAction("Index");
        }
    }
}