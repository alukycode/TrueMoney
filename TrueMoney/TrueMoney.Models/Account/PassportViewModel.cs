using System;
using System.ComponentModel.DataAnnotations;
using TrueMoney.Common;

namespace TrueMoney.Models.Account
{
    public class PassportViewModel
    {
        [Required(ErrorMessage = ErrorMessages.Required)]
        [RegularExpression(".{6,10}", ErrorMessage = ErrorMessages.Invalid)]
        [Display(Name="Номер паспорта")]
        public string Number { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [DataType(DataType.Date, ErrorMessage = ErrorMessages.Invalid)]
        [Display(Name="Дата выдачи")]
        public DateTime DateOfIssue { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [Display(Name = "Орган, выдавший паспорт")]
        public string GiveOrganization { get; set; }
    }
}