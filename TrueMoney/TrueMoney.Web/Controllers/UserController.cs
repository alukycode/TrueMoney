using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrueMoney.Infrastructure.Services;

namespace TrueMoney.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<string> Index()
        {
            var res = await _userService.GetAll();
            return "Controller " + res.First().Name;
        }
    }
}