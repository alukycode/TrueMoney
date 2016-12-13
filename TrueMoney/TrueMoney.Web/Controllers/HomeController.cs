using System.Web.Mvc;
using TrueMoney.Services;

namespace TrueMoney.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Rating()
        {
            return Redirect(Url.Action("About") + "#rating" );
        }
    }
}