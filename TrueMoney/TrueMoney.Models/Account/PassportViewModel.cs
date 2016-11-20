using System;
using System.ComponentModel.DataAnnotations;
using TrueMoney.Common;

namespace TrueMoney.Models.Account
{
    public class PassportViewModel
    {
        [Required(ErrorMessage = ErrorMessages.Required)]
        [RegularExpression(".{6,10}", ErrorMessage = ErrorMessages.Invalid)]
        [Display(Name="����� ��������")]
        public string Number { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [DataType(DataType.Date, ErrorMessage = ErrorMessages.Invalid)]
        [Display(Name="���� ������")]
        public DateTime DateOfIssue { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [Display(Name = "�����, �������� �������")]
        public string GiveOrganization { get; set; }
    }
}