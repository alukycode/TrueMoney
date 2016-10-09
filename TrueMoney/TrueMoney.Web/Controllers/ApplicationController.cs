namespace TrueMoney.Web.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Mvc;

    using TrueMoney.Infrastructure.Services;

    [Authorize]
    public class ApplicationController : Controller
    {
        private readonly IApplicationService applicationService;
        private readonly IUserService userService;

        public ApplicationController(IApplicationService applicationService,
            IUserService userService)
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
            this.ViewBag.CurrentUser = await this.userService.GetCurrentUser();
            await this.applicationService.ApplyOffer(offerId, appId);

            return this.RedirectToAction("Details", new { id = appId });
        }
    }
}