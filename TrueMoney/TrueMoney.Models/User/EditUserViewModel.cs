using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueMoney.Models.Basic;

namespace TrueMoney.Models.User
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Display(Name = "Номер банковского счета")] //todo: нужна валидация
        public string BankAccountNumber { get; set; }
    }
}
