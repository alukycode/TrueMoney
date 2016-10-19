using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TrueMoney.Models.Basic;

namespace TrueMoney.Web.Controllers
{
    public class BaseController : Controller
    {
        // todo: async await?
        public int CurrentUserId
        {
            get
            {
                return 1;

                //int userId;
                //if (Int32.TryParse(Request.QueryString["user"], out userId))
                //{
                //    return new UserModel { Id = userId, IsActive = true,};
                //}
                //var aspNetId = User.Identity.GetUserId();
                //// var user = _userService.GetUserByAspNetId(aspNetId);
                //// return user;
                //return new UserModel { Id = 1, IsActive = true, };
            }
        }

        protected ActionResult GoHome()
        {
            return RedirectToAction("YouActivity", "Loan");
        }
    }
}