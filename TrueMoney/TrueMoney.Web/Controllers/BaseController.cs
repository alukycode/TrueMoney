using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TrueMoney.Infrastructure.Entities;

namespace TrueMoney.Web.Controllers
{
    public class BaseController : Controller
    {
        // todo: async await?
        public User CurrentUser
        {
            get
            {
                int userId;
                if (Int32.TryParse(Request.QueryString["user"], out userId))
                {
                    return new User { Id = userId, IsActive = true, IsHaveOpenDealOrLoan = false };
                }
                var aspNetId = User.Identity.GetUserId();
                // var user = _userService.GetUserByAspNetId(aspNetId);
                // return user;
                return new User { Id = 1, IsActive = true, IsHaveOpenDealOrLoan = false };
            }
        }

        protected ActionResult GoHome()
        {
            return RedirectToAction("YouActivity", "Loan");
        }
    }
}