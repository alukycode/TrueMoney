using System.ComponentModel.DataAnnotations;
using TrueMoney.Common;

namespace TrueMoney.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = ErrorMessages.Required)]
        [EmailAddress(ErrorMessage = ErrorMessages.Invalid)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [DataType(DataType.Password)]
        [Display(Name = "������")]
        public string Password { get; set; }

        [Display(Name = "��������� ����?")]
        public bool RememberMe { get; set; }
    }
}