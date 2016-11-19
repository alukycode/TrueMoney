using System;
using System.Linq;
using TrueMoney.Common.Enums;
using TrueMoney.Models;
using TrueMoney.Services;

namespace TrueMoney.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Models.Offer;
    using Services.Interfaces;

    using TrueMoney.Common;
    using TrueMoney.Models.Deal;

    [Authorize(Roles = RoleNames.User)]
    public class DealController : Controller
    {
        private readonly IDealService _dealService;

        public DealController(IDealService dealService)
        {
            _dealService = dealService;
        }

        [AllowAnonymous]
        public async Task<ActionResult> Index()
        {
            DealIndexViewModel model;
            model = await _dealService.GetAll(User.Identity.GetUserId<int>());

            return View(model);
        }

        public async Task<ActionResult> Details(int id)
        {
            var model = await _dealService.GetById(id, User.Identity.GetUserId<int>());
            if (model != null)
            {
                return View(model);
            }

            return RedirectToAction("Index"); //TODO: надо тут ошибку отображать на самом деле
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FinishDeal(int offerId, int dealId)
        {
            await _dealService.FinishDealStartLoan(offerId);

            return RedirectToAction("Details", "Deal", new { id = dealId });
        }

        public async Task<ActionResult> Create()
        {
            var viewModel = await _dealService.GetCreateDealForm();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateDealForm model)
        {
            if (model.PaymentCount < 1 && model.PaymentCount > model.DealPeriod)
            {
                ModelState.AddModelError("PaymentCount", "Неверное количество платежей.");
            }

            if (ModelState.IsValid)
            {
                model.OwnerId = User.Identity.GetUserId<int>();
                var id = await _dealService.CreateDeal(model);

                return RedirectToAction("Details", new { id = id });
            }

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int dealId)
        {
            await _dealService.DeleteDeal(dealId, User.Identity.GetUserId<int>());

            return RedirectToAction("Index");
        }
    }
}