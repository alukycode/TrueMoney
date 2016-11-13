using System.Web.Mvc;
using TrueMoney.Services;

namespace TrueMoney.Web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
        }

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
            return View();
        }
    }
}