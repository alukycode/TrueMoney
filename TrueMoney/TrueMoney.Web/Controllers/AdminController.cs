using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrueMoney.Common;
using TrueMoney.Services.Interfaces;

namespace TrueMoney.Web.Controllers
{
    using Microsoft.AspNet.Identity;

    [Authorize(Roles = RoleNames.Admin)]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IDealService _dealService;
        private readonly IPaymentService _paymentService;

        public AdminController(IUserService userService, IDealService dealService, IPaymentService paymentService)
        {
            _userService = userService;
            _dealService = dealService;
            _paymentService = paymentService;
        }

        public async Task<ActionResult> InactiveUsers()
        {
            var model = await _userService.GetInactiveUsersViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Activate(int userId)
        {
            await _userService.ActivateUser(userId);
            return RedirectToAction("InactiveUsers");
        }

        [HttpPost]
        public async Task<ActionResult> Deactivate(int userId)
        {
            await _userService.DeactivateUser(userId);
            return RedirectToAction("InactiveUsers");
        }


        public async Task<ActionResult> DealList()
        {
            var model = await _dealService.GetDealListViewModel();
            return View(model);
        }

        public async Task<ActionResult> DeleteDeal(int dealId, int userId)
        {
            await _dealService.DeleteDeal(dealId, userId);
            return RedirectToAction("DealList");
        }

        public async Task<ActionResult> TransactionList()
        {
            var model = await _paymentService.AdminGetBankTransactions();
            return View(model);
        }
    }
}