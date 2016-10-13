using TrueMoney.Web.Models;

namespace TrueMoney.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using Infrastructure.Services;

    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly IApplicationService _applicationService;

        private readonly IUserService _userService;

        public ApplicationController(IApplicationService applicationService, IUserService userService)
        {
            _applicationService = applicationService;
            _userService = userService;
        }
        
        public async Task<ActionResult> Index()
        {
            this.ViewBag.CurrentUser = await this._userService.GetCurrentUser();
            var list = await this._applicationService.GetAll();
            return this.View(list.Where(x => !x.IsClosed));
        }

        public async Task<ActionResult> Details(int id)
        {
            ViewBag.CurrentUser = await _userService.GetCurrentUser();
            var model = await _applicationService.GetById(id);
            return View(model);
        }

        public async Task<ActionResult> ApplyOffer(int offerId, int appId)
        {
            await _applicationService.ApplyOffer(offerId, appId);

            return RedirectToAction("Details", new { id = appId });
        }

        public async Task<ActionResult> RevertOffer(int offerId, int appId)
        {
            await this._applicationService.RevertOffer(offerId, appId);

            return RedirectToAction("Details", new { id = appId });
        }

        public async Task<ActionResult> FinishApp(int offerId, int appId)
        {
            var newLoanId = await _applicationService.FinishApp(offerId, appId);

            return RedirectToAction("Details", "Loan", new { id = newLoanId });
        }

        public async Task<ActionResult> Create()
        {
            return View(new CreateMoneyApplicationForm());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateMoneyApplicationForm model)
        {
            var appId =
                await _applicationService.CreateApp(model.Count, model.Rate, model.DayCount, model.Description);

            return RedirectToAction("Details", new { id = appId });
        }

        [HttpPost]
        public async Task<ActionResult> CreateOffer(CreateOfferForm model)
        {
            if (ModelState.IsValid)
            {
                var app = await _applicationService.GetById(model.AppId);
                var currentUser = await _userService.GetCurrentUser();
                ViewBag.CurrentUser = currentUser;
                if (model.Rate > app.Rate)
                {
                    ModelState.AddModelError("Rate", "Вы превысили маскимальнодопустимую процентную ставку.");
                    return View("Details", app);
                }

                if (!currentUser.IsActive)
                {
                    ModelState.AddModelError("User error", "Вы ещё не прошли подтверждение регистрации.");
                    return View("Details", app);
                }

                var res = await _applicationService.CreateOffer(model.AppId, model.Rate);
                if (!res)
                {
                    ModelState.AddModelError("Server error", "Что-то пошло не так.");
                    return View("Details", app);
                }
            }

            return RedirectToAction("Details", new { id = model.AppId });
        }

        public async Task<ActionResult> Delete(int appId)
        {
            var res = await _applicationService.DeleteApp(appId);

            if (res)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Details", new { id = appId });
        }
    }
}