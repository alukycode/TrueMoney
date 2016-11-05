using System;
using System.Linq;
using TrueMoney.Common.Enums;
using TrueMoney.Models;
using TrueMoney.Services;

namespace TrueMoney.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Services.Interfaces;

    using TrueMoney.Models.Deal;
    using TrueMoney.Models.ViewModels;

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
            try
            {
                var currentUserId = CurrentUserId;
                model = await _dealService.GetAll(currentUserId);
            }
            catch (Exception)
            {
                model = await _dealService.GetAllForAnonymous();
            }

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
        public async Task<ActionResult> ApproveOffer(int offerId) 
        {
            await _dealService.ApproveOffer(offerId);

            return null; // потом еще сделаем результат
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<ActionResult> CancelOfferApproval(int offerId)
        {
            await _dealService.CancelOfferApproval(offerId);

            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RevertOffer(int offerId, int dealId) 
        {
            await _dealService.RevertOffer(offerId);

            return RedirectToAction("Details", new { id = dealId });
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

        public async Task<ActionResult> CreateOffer(int dealId)
        {
            var formModel = await _dealService.GetCreateOfferForm(dealId, CurrentUserId);
            if (formModel.IsUserCanCreateOffer)
            {
                return View(formModel);
            }

            return GoHome();
        }

        [HttpPost]
        public async Task<ActionResult> CreateOffer(CreateOfferForm model)
        {
            if (model.DealRate < model.InterestRate)
            {
                ModelState.AddModelError("InterestRate", "Вы превысили маскимальнодопустимую процентную ставку.");
            }

            if (!model.IsUserCanCreateOffer)
            {
                ModelState.AddModelError("InterestRate", "Вы ещё не прошли подтверждение регистрации.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _dealService.CreateOffer(model, CurrentUserId); 
                    return RedirectToAction("Details", new { id = model.DealId });// во, редирект, а не создание той же модели и отрисовка той же вьюшки!
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("InterestRate", "Что-то пошло не так.");
                }
            }

            return View("CreateOffer", model);
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