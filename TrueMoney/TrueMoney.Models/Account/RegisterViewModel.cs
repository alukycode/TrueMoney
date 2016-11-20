using System.ComponentModel.DataAnnotations;
using TrueMoney.Common;

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

        public PassportViewModel Passport { get; set; }

        //[Required]
        //[RegularExpression("[0-9]{3}.[0-9]{2}.[0-9]{3}.[0-9]{1}.[0-9]{4}.[0-9]{7}")] // https://ru.wikipedia.org/wiki/%D0%A0%D0%B0%D1%81%D1%87%D1%91%D1%82%D0%BD%D1%8B%D0%B9_%D1%81%D1%87%D1%91%D1%82
        [Display(Name="Номер банковского счёта")]
        public string BankAccountNumber { get; set; }
    }
}