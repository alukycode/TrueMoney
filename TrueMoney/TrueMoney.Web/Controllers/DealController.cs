using TrueMoney.Web.Models;

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
            ViewBag.CurrentUserId = CurrentUser.Id;
            if (id.HasValue)
            {
                var model = await _dealService.GetById(id.Value);
                return View(model);
            }

            return GoHome();
        }

        public async Task<ActionResult> ApplyOffer(int? offerId, int? dealId)
        {
            if (offerId.HasValue && dealId.HasValue)
            {
                await _dealService.ApplyOffer(CurrentUser, offerId.Value, dealId.Value);

                return RedirectToAction("Details", new { id = dealId.Value });
            }

            return GoHome();
        }

        public async Task<ActionResult> RevertOffer(int? offerId, int? dealId)
        {
            if (offerId.HasValue && dealId.HasValue)
            {
                await this._dealService.RevertOffer(CurrentUser, offerId.Value, dealId.Value);

                return RedirectToAction("Details", new { id = dealId.Value });
            }

            return GoHome();
        }

        public async Task<ActionResult> FinishDeal(int? offerId, int? dealId)
        {
            if (offerId.HasValue && dealId.HasValue)
            {
                var newLoanId = await _dealService.FinishDeal(CurrentUser, offerId.Value, dealId.Value);
                if (newLoanId > -1)
                {
                    return RedirectToAction("Details", "Loan", new { id = newLoanId });
                }
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

                var dealId =
                    await _dealService.CreateDeal(CurrentUser, model.Count, model.PaymentCount, model.Rate, model.DayCount, model.Description);
                if (dealId > -1)
                {
                    return RedirectToAction("Details", new { id = dealId });
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOffer(CreateOfferForm model)
        {
            if (ModelState.IsValid)
            {
                var deal = await _dealService.GetById(model.DealId);
                if (model.Rate > deal.Rate)
                {
                    ModelState.AddModelError("Rate", "Вы превысили маскимальнодопустимую процентную ставку.");
                    return View("Details", deal);
                }

                if (!CurrentUser.IsActive)
                {
                    ModelState.AddModelError("", "Вы ещё не прошли подтверждение регистрации.");
                    return View("Details", deal);
                }

                var res = await _dealService.CreateOffer(CurrentUser, model.DealId, model.Rate);
                if (!res)
                {
                    ModelState.AddModelError("", "Что-то пошло не так.");
                    return View("Details", deal);
                }
            }

            return RedirectToAction("Details", new { id = model.DealId });
        }

        public async Task<ActionResult> Delete(int? dealId)
        {
            if (dealId.HasValue)
            {
                var res = await _dealService.DeleteDeal(CurrentUser, dealId.Value);

                if (res)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Details", new { id = dealId });
        }
    }
}