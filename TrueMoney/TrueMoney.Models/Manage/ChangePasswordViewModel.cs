using System.ComponentModel.DataAnnotations;
using TrueMoney.Common;

namespace TrueMoney.Models.Manage
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = ErrorMessages.Required)]
        [DataType(DataType.Password)]
        [Display(Name = "Текущий пароль")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(100, ErrorMessage = "{0} должен быть минимум {2} символа.", MinimumLength = 1)] //TODO: как-то это надо совмещать с общеми правилами валидации пароля
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повторно введите новый пароль")]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}