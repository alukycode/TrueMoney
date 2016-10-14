﻿using TrueMoney.Web.Models;

namespace TrueMoney.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Infrastructure.Services;

    [Authorize]
    public class DealController : BaseController
    {
        private readonly IDealService _dealService;

        private readonly IUserService _userService;

        public DealController(IDealService dealService, IUserService userService)
        {
            _dealService = dealService;
            _userService = userService;
        }
        
        public async Task<ActionResult> Index()
        {
            var list = await this._dealService.GetAll();
            return this.View(list.Where(x => !x.IsClosed));
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id.HasValue)
            {
                var model = await _dealService.GetById(id.Value);
                return View(model);
            }

            return GoHome();
        }

        public async Task<ActionResult> ApplyOffer(int? offerId, int? appId)
        {
            if (offerId.HasValue && appId.HasValue)
            {
                await _dealService.ApplyOffer(offerId.Value, appId.Value);

                return RedirectToAction("Details", new { id = appId.Value });
            }

            return GoHome();
        }

        public async Task<ActionResult> RevertOffer(int? offerId, int? appId)
        {
            if (offerId.HasValue && appId.HasValue)
            {
                await this._dealService.RevertOffer(offerId.Value, appId.Value);

                return RedirectToAction("Details", new { id = appId.Value });
            }

            return GoHome();
        }

        public async Task<ActionResult> FinishApp(int? offerId, int? appId)
        {
            if (offerId.HasValue && appId.HasValue)
            {
                var newLoanId = await _dealService.FinishApp(CurrentUser, offerId.Value, appId.Value);
                if (newLoanId > -1)
                {
                    return RedirectToAction("Details", "Loan", new { id = newLoanId });
                }
            }

            return GoHome();
        }

        public async Task<ActionResult> Create()
        {
            return View(new CreateMoneyApplicationForm());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateMoneyApplicationForm model)
        {
            if (model.PaymentCount < 1 && model.PaymentCount > model.DayCount)
            {
                ModelState.AddModelError("PaymentCount", "Неверное количество платежей.");
            }
            if (ModelState.IsValid)
            {

                var appId =
                    await _dealService.CreateApp(CurrentUser, model.Count, model.PaymentCount, model.Rate, model.DayCount, model.Description);
                if (appId > -1)
                {
                    return RedirectToAction("Details", new { id = appId });
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOffer(CreateOfferForm model)
        {
            if (ModelState.IsValid)
            {
                var app = await _dealService.GetById(model.AppId);
                if (model.Rate > app.Rate)
                {
                    ModelState.AddModelError("Rate", "Вы превысили маскимальнодопустимую процентную ставку.");
                    return View("Details", app);
                }

                if (!CurrentUser.IsActive)
                {
                    ModelState.AddModelError("User error", "Вы ещё не прошли подтверждение регистрации.");
                    return View("Details", app);
                }

                var res = await _dealService.CreateOffer(CurrentUser, model.AppId, model.Rate);
                if (!res)
                {
                    ModelState.AddModelError("Server error", "Что-то пошло не так.");
                    return View("Details", app);
                }
            }

            return RedirectToAction("Details", new { id = model.AppId });
        }

        public async Task<ActionResult> Delete(int? appId)
        {
            if (appId.HasValue)
            {
                var res = await _dealService.DeleteApp(CurrentUser, appId.Value);

                if (res)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Details", new { id = appId });
        }

        private ActionResult GoHome()
        {
            return RedirectToAction("YouActivity", "Loan");
        }
    }
}