﻿using System.Web.Mvc;

namespace TrueMoney.Web.Controllers
{
    public class HomeController : BaseController
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

        public ActionResult Borrow()
        {
            return View();
        }

        public ActionResult Lend()
        {
            return View();
        }
    }
}