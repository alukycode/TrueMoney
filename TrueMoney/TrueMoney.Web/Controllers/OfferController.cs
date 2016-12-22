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
    using Microsoft.AspNet.Identity;
    using TrueMoney.Common;

    [Authorize(Roles = RoleNames.User)]
    public class OfferController : Controller
    {
        private readonly IOfferService _offerService;

        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpPost]
        public async Task ApproveOffer(int offerId)
        {
            await _offerService.ApproveOffer(offerId, User.Identity.GetUserId<int>());
        }

        [HttpPost]
        public async Task CancelOfferApproval(int offerId)
        {
            await _offerService.CancelOfferApproval(offerId, User.Identity.GetUserId<int>());
        }

        [HttpPost]
        public async Task RevertOffer(int dealId)
        {
            await _offerService.RevertOffer(dealId, User.Identity.GetUserId<int>());
        }

        public async Task<ActionResult> CreateOffer(int dealId)
        {
            var formModel = await _offerService.GetCreateOfferForm(dealId, User.Identity.GetUserId<int>());
            return View(formModel);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOffer(CreateOfferForm model)
        {
            if (model.DealRate < model.InterestRate)
            {
                ModelState.AddModelError(nameof(model.InterestRate), "Вы превысили максимально допустимую процентную ставку.");
            }

            if (ModelState.IsValid)
            {
                await _offerService.CreateOffer(model, User.Identity.GetUserId<int>());
                return RedirectToAction("Details", "Deal", new { id = model.DealId });
            }

            return View(model);
        }
    }
}