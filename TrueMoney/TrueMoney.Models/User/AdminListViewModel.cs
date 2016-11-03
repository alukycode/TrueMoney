namespace TrueMoney.Models.User
{
    using System.Collections.Generic;

    public class AdminListViewModel
    {
        public IList<UserAndPassportViewModel> Users { get; set; } 
    }
}