namespace TrueMoney.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using TrueMoney.Infrastructure.Services;
    using TrueMoney.Web.Models.ViewModel;

    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly IApplicationService applicationService;

        private readonly IUserService userService;

        public ApplicationController(IApplicationService applicationService, IUserService userService)
        {
            this.applicationService = applicationService;
            this.userService = userService;
        }

        public async Task<ActionResult> Index()
        {
            this.ViewBag.CurrentUser = await this.userService.GetCurrentUser();
            var list = await this.applicationService.GetAll();
            return this.View(list);
        }

        public async Task<ActionResult> Details(int id)
        {
            this.ViewBag.CurrentUser = await this.userService.GetCurrentUser();
            var model = await this.applicationService.GetById(id);
            return this.View(model);
        }

        public async Task<ActionResult> ApplyOffer(int offerId, int appId)
        {
            await this.applicationService.ApplyOffer(offerId, appId);

            return this.RedirectToAction("Details", new { id = appId });
        }

        public async Task<ActionResult> RevertOffer(int offerId, int appId)
        {
            await this.applicationService.ApplyOffer(offerId, appId);

            return this.RedirectToAction("Details", new { id = appId });
        }

        public async Task<ActionResult> FinishApp(int offerId, int appId)
        {
            var newLoanId = await this.applicationService.FinishApp(offerId, appId);

            return this.RedirectToAction("Details", "Loan", new { id = newLoanId });
        }

        public async Task<ActionResult> Create()
        {
            return this.View(new CreateMoneyApplicationForm());
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateMoneyApplicationForm model)
        {
            var appId =
                await this.applicationService.CreateApp(model.Count, model.Rate, model.DayCount, model.Description);

            return this.RedirectToAction("Details", new { id = appId });
        }

        [HttpPost]
        public async Task<ActionResult> CreateOffer(CreateOfferForm model)
        {
            if (ModelState.IsValid)
            {
                var app = await this.applicationService.GetById(model.AppId);
                var currentUser = await this.userService.GetCurrentUser();
                this.ViewBag.CurrentUser = currentUser;
                if (model.Rate > app.Rate)
                {
                    ModelState.AddModelError("Rate", "Вы превысили маскимальнодопустимую процентную ставку.");
                    return this.View("Details", app);
                }

                if (!currentUser.IsActive)
                {
                    ModelState.AddModelError("User error", "Вы ещё не прошли подтверждение регистрации.");
                    return this.View("Details", app);
                }

                var res = await this.applicationService.CreateOffer(model.AppId, model.Rate);
                if (!res)
                {
                    ModelState.AddModelError("Server error", "Что-то пошло не так.");
                    return this.View("Details", app);
                }
            }

            return this.RedirectToAction("Details", new { id = model.AppId });
        }
    }
}