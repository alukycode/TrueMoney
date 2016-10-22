using System;
using System.Linq;
using TrueMoney.Common.Enums;
using TrueMoney.Models;
using TrueMoney.Services;

namespace TrueMoney.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;

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
            var list = await this._dealService.GetAllOpen(await CurrentUserId());
            return this.View(list);
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

        //я пока закоммитаю то, что не компилиться
        //public async Task<ActionResult> ApplyOffer(int? offerId, int? dealId)
        //{
        //    if (offerId.HasValue && dealId.HasValue)
        //    {
        //        await _dealService.ApplyOffer(CurrentUser, offerId.Value, dealId.Value);

        //        return RedirectToAction("Details", new { id = dealId.Value });
        //    }

        //    return GoHome();
        //}

        //public async Task<ActionResult> RevertOffer(int? offerId, int? dealId)
        //{
        //    if (offerId.HasValue && dealId.HasValue)
        //    {
        //        await this._dealService.RevertOffer(CurrentUser, offerId.Value, dealId.Value);

        //        return RedirectToAction("Details", new { id = dealId.Value });
        //    }

        //    return GoHome();
        //}

        public async Task<ActionResult> FinishDeal(int? offerId, int? dealId)
        {
            if (offerId.HasValue && dealId.HasValue)
            {
                await _dealService.FinishDealStartLoan(await CurrentUserId(), offerId.Value, dealId.Value);

                return RedirectToAction("Details", "Deal", new { id = dealId.Value });
            }

            return GoHome();
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
                var deal = await _dealService.CreateDeal(model, await CurrentUserId());

                return RedirectToAction("Details", new { id = deal.Id });
            }

            return View(model);
        }

        public async Task<ActionResult> CreateOffer(int dealId)
        {
            var formModel = await _dealService.GetCreateOfferForm(dealId, await CurrentUserId());
            if (formModel.IsUserCanCreateOffer)
            {
                return View("CreateOfferForm", formModel);
            }

            return GoHome();
        }

        [HttpPost]
        public async Task<ActionResult> CreateOffer(CreateOfferForm model)
        {
            if (model.DealRate > model.Rate)
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
                    var deal = await _dealService.CreateOffer(model, await CurrentUserId());
                    return View("Details", deal);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Что-то пошло не так.");
                }
            }

            return RedirectToAction("Details", new { id = model.DealId });
        }

        //public async Task<ActionResult> Delete(int? dealId)
        //{
        //    if (dealId.HasValue)
        //    {
        //        var res = await _dealService.DeleteDeal(CurrentUser, dealId.Value);

        //        if (res)
        //        {
        //            return RedirectToAction("Index");
        //        }
        //    }

        //    return RedirectToAction("Details", new { id = dealId });
        //}

        public async Task<ActionResult> YouActivity()
        {
            var viewModel = new YouActivityViewModel
            {
                Deals = await _dealService.GetAllByUser(await CurrentUserId()),
                Offers = await _dealService.GetAllOffersByUser(await CurrentUserId())
            };

            return View(viewModel);
        }
    }
}