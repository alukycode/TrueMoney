using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace TrueMoney.Models.Manage
{
    // class used on page where we can manage external logins (facebook etc)
    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        // we need whole owin in this project only because of this property
        public IList<AuthenticationDescription> OtherLogins { get; set; } 
    }
}