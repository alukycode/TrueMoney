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

    using TrueMoney.Models.Deal;

    [Authorize]
    public class DealController : BaseController
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
            model = await _dealService.GetAll(CurrentUserId);

            return View(model);
        }

        public async Task<ActionResult> Details(int id)
        {
            var model = await _dealService.GetById(id, CurrentUserId);
            if (model != null)
            {
                return View(model);
            }

            return GoHome();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FinishDeal(int offerId, int dealId)
        {
            await _dealService.FinishDealStartLoan(CurrentUserId, offerId, dealId);

            return RedirectToAction("Details", "Deal", new { id = dealId });
        }

        public async Task<ActionResult> Create()
        {
            var viewModel = await _dealService.GetCreateDealForm(CurrentUserId);
            if (viewModel.IsUserCanCreateDeal)
            {
                return View(new CreateDealForm());
            }

            return GoHome();
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
                var id = await _dealService.CreateDeal(model, CurrentUserId);

                return RedirectToAction("Details", new { id = id });
            }

            return View(model);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int dealId)
        {
            await _dealService.DeleteDeal(dealId, CurrentUserId);

            return GoHome();
        }

        public async Task<ActionResult> YourActivity() // для меня загадка, почему активностью юзера занимается контроллер сделок
        {
            var viewModel = await _dealService.GetYourActivityViewModel(CurrentUserId);

            return View(viewModel);
        }
    }
}