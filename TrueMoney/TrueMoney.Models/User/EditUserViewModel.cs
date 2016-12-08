using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueMoney.Common;
using TrueMoney.Models.Basic;

namespace TrueMoney.Models.User
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [EmailAddress(ErrorMessage = ErrorMessages.Invalid)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Имя")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        public string LastName { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Display(Name = "Номер банковского счета")] //todo: нужна валидация
        [Required(ErrorMessage = ErrorMessages.Required)]
        public string BankAccountNumber { get; set; }
    }
}
