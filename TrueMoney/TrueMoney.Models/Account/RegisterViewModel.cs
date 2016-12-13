using System.ComponentModel.DataAnnotations;
using TrueMoney.Common;
using TrueMoney.Models.Basic;

namespace TrueMoney.Models.Account
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = ErrorMessages.Required)]
        [EmailAddress(ErrorMessage = ErrorMessages.Invalid)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(100, ErrorMessage = "{0} не может быть короче, чем {2} символ", MinimumLength = 1)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Пароль (подтверждение)")]
        [Compare(nameof(Password), ErrorMessage = "Введенные пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [Display(Name="Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [Display(Name="Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        public PassportModel Passport { get; set; }

        [Required]
        [RegularExpression("^[0-9]{16}$", ErrorMessage = "Номер карты состоит из 16 цифр.")]
        [Display(Name="Номер банковской карты")]
        public string CardNumber { get; set; }
    }
}