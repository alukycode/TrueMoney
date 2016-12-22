namespace TrueMoney.Models.User
{
    using TrueMoney.Models.Basic;

    public class UserAndPassportViewModel
    {
        public UserModel User { get; set; } 

        public PassportModel Passport { get; set; }

        public bool IsUserCanBeDeactivated { get; set; }

        public int CountOfAllDeals { get; set; }

        public int CountOfDealsInProgress { get; set; }

        public int CountOfAllOffers { get; set; }

        public int CountOfApprovedOffers { get; set; }
    }
}