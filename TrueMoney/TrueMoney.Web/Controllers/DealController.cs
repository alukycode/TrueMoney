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
    using TrueMoney.Models.ViewModels;

    [Authorize]
    public class DealController : BaseController
    {
        private readonly IDealService _dealService;

        public DealController(IDealService dealService)
        {
            _dealService = dealService;
        }

        public async Task<ActionResult> Index()
        {
            var model = await _dealService.GetAllOpen(await CurrentUserId());
            return View(model);
        }

        public async Task<ActionResult> Details(int id)
        {
            var model = await _dealService.GetById(id, await CurrentUserId());
            if (model != null)
            {
                return View(model);
            }

            return GoHome();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //збс место для аджаксовых запросов
        public async Task<ActionResult> ApproveOffer(int offerId) 
        {
            await _dealService.ApproveOffer(offerId);

            return null; // хз, че тут пока что возвращать
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RevertOffer(int offerId, int dealId) // надо тоже в аджакс хуйнуть
        {
            await _dealService.RevertOffer(offerId);

            return RedirectToAction("Details", new { id = dealId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FinishDeal(int offerId, int dealId)
        {
            await _dealService.FinishDealStartLoan(await CurrentUserId(), offerId, dealId);

            return RedirectToAction("Details", "Deal", new { id = dealId });
        }

        public async Task<ActionResult> Create()
        {
            return View(new CreateDealForm());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateDealForm model)
        {
            if (model.PaymentCount < 1 && model.PaymentCount > model.DayCount)
            {
                ModelState.AddModelError("PaymentCount", "Неверное количество платежей.");
            }

            if (ModelState.IsValid)
            {
                var id = await _dealService.CreateDeal(model, await CurrentUserId());

                return RedirectToAction("Details", new { id = id });
            }

            return View(model);
        }

        public async Task<ActionResult> CreateOffer(int dealId)
        {
            var formModel = await _dealService.GetCreateOfferForm(dealId, await CurrentUserId());
            if (formModel.IsUserCanCreateOffer)
            {
                return View(formModel);
            }

            return GoHome();
        }

        [HttpPost]
        public async Task<ActionResult> CreateOffer(CreateOfferForm model)
        {
            if (model.DealRate > model.InterestRate)
            {
                ModelState.AddModelError("Rate", "Вы превысили маскимальнодопустимую процентную ставку.");
            }

            if (!model.IsUserCanCreateOffer)
            {
                ModelState.AddModelError("", "Вы ещё не прошли подтверждение регистрации.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _dealService.CreateOffer(model, await CurrentUserId()); 
                    return RedirectToAction("Details", new { id = model.DealId });// во, редирект, а не создание той же модели и отрисовка той же вьюшки!
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Что-то пошло не так.");
                }
            }

            return RedirectToAction("Details", new { id = model.DealId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int dealId)
        {
            await _dealService.DeleteDeal(dealId, await CurrentUserId());

            return GoHome();
        }

        public async Task<ActionResult> YourActivity() // для меня загадка, почему активностью юзера занимается контроллер сделок
        {
            var viewModel = await _dealService.GetYourActivityViewModel(await CurrentUserId());

            return View(viewModel);
        }
    }
}