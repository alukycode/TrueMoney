using System.ComponentModel.DataAnnotations;

namespace TrueMoney.Models.Account
{
    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            Passport = new PassportViewModel();
        }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 1)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public PassportViewModel Passport { get; set; }

        //[Required]
        //[RegularExpression("[0-9]{3}.[0-9]{2}.[0-9]{3}.[0-9]{1}.[0-9]{4}.[0-9]{7}")] // https://ru.wikipedia.org/wiki/%D0%A0%D0%B0%D1%81%D1%87%D1%91%D1%82%D0%BD%D1%8B%D0%B9_%D1%81%D1%87%D1%91%D1%82
        public string BankAccountNumber { get; set; }

        public string AspUserId { get; set; }
    }
}