using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrueMoney.Models.Offer;
using TrueMoney.Services.Interfaces;

namespace TrueMoney.Web.Controllers
{
    using TrueMoney.Common;

    [Authorize(Roles = RoleNames.User)]
    public class OfferController : BaseController
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task ApproveOffer(int offerId)
        {
            await _offerService.ApproveOffer(offerId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task CancelOfferApproval(int offerId)
        {
            await _offerService.CancelOfferApproval(offerId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RevertOffer(int offerId, int dealId)
        {
            await _offerService.RevertOffer(offerId, CurrentUserId);

            return RedirectToAction("Details", "Deal", new { id = dealId });
        }

        public async Task<ActionResult> CreateOffer(int dealId)
        {
            var formModel = await _offerService.GetCreateOfferForm(dealId, CurrentUserId);
            return View(formModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOffer(CreateOfferForm model)
        {
            if (model.DealRate < model.InterestRate) // это нужно убрать отсуюда, здесь уже все проверено должно быть
            {
                ModelState.AddModelError("InterestRate", "Вы превысили маскимальнодопустимую процентную ставку.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _offerService.CreateOffer(model);
                    return RedirectToAction("Details", "Deal", new { id = model.DealId });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("InterestRate", "Что-то пошло не так.");
                }
            }

            return View(model);
        }
    }
}