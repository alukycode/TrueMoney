using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueMoney.Models.Basic
{
    public class UserModel
    {
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Номер банковской карты")] 
        [RegularExpression("^[0-9]{16}$", ErrorMessage = "Номер карты состоит из 16 цифр.")]
        public string CardNumber { get; set; }

        public int Rating { get; set; }

        public int? PassportId { get; set; }

        public string Email { get; set; }
    }
}
