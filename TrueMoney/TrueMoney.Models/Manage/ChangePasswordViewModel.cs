using System.ComponentModel.DataAnnotations;
using TrueMoney.Common;

namespace TrueMoney.Models.Manage
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = ErrorMessages.Required)]
        [DataType(DataType.Password)]
        [Display(Name = "������� ������")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [StringLength(100, ErrorMessage = "{0} ������ ���� ������� {2} �������.", MinimumLength = 1)] //TODO: ���-�� ��� ���� ��������� � ������ ��������� ��������� ������
        [DataType(DataType.Password)]
        [Display(Name = "����� ������")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "�������� ������� ����� ������")]
        [Compare("NewPassword", ErrorMessage = "������ �� ���������.")]
        public string ConfirmPassword { get; set; }
    }
}