using System.Collections.Generic;
using TrueMoney.Models.User;

namespace TrueMoney.Models.Admin
{
    public class InactiveUsersViewModel
    {
        public IList<UserAndPassportViewModel> Users { get; set; } 
    }
}