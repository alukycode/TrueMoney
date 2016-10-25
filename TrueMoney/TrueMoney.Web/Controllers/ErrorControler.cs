using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TrueMoney.Common;

namespace TrueMoney.Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Code(int id, string exceptionMessage)
        {
            //Response.TrySkipIisCustomErrors = true;

            // comment when application will be production-ready
            ViewBag.ExceptionMessage = exceptionMessage;

            Response.StatusCode = id;

            switch (id)
            {
                case 404:
                    return View("404");
                case 500:
                    return View("500");
            }

            return View("666");
        }

        public ActionResult Test(int id)
        {
            throw new Exception();
        }
    }
}