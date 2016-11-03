using System;
using System.ComponentModel.DataAnnotations;

namespace TrueMoney.Models.Account
{
    public class PassportViewModel
    {
        [Required]
        [RegularExpression("[0-9]{4}")]
        public string Series { get; set; }

        [Required]
        [RegularExpression("[0-9]{6}")]
        public string Number { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfIssuing { get; set; }

        [Required]
        public string GiveOrganisation { get; set; }
    }
}