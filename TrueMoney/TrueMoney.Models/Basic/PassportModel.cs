namespace TrueMoney.Models.Basic
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Common;

    public class PassportModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [RegularExpression(".{6,10}", ErrorMessage = ErrorMessages.Invalid)]
        [Display(Name = "Номер паспорта")]
        public string Number { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [DataType(DataType.Date, ErrorMessage = ErrorMessages.Invalid)]
        [Display(Name = "Дата выдачи")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfIssuing { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [Display(Name = "Орган, выдавший паспорт")]
        public string GiveOrganization { get; set; }
    }
}