namespace TrueMoney.Models.User
{
    using TrueMoney.Models.Basic;

    public class UserAndPassportViewModel
    {
        public UserModel User { get; set; } 

        public PassportModel Passport { get; set; }
    }
}