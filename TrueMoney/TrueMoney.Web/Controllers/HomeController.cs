using System.Web.Mvc;
using TrueMoney.Services;

namespace TrueMoney.Web.Controllers
{
    public class HomeController : Controller //TODO: надо убрать лишнее
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