using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrueMoney.Web.Controllers
{
    using System.Threading.Tasks;

    public class LoanController : Controller
    {
        public async Task<ActionResult> Index()
        {
            return new EmptyResult();
        }

        public async Task<ActionResult> Details(int id)
        {
            return new EmptyResult();
        }
    }
}