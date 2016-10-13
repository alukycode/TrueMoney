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
                var aspNetId = User.Identity.GetUserId();
                // var user = _userService.GetUserByAspNetId(aspNetId);
                // return user;
                return new User { Id = 1 };
            }
        }
    }
}