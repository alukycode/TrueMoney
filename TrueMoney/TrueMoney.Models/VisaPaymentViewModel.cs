using System;
using System.ComponentModel.DataAnnotations;

namespace TrueMoney.Models
{
    public class VisaPaymentViewModel
    {
        public string PaymentName { get; set; }

        [Required]
        public decimal PaymentCount { get; set; }

        public bool CanSetPaymentCount { get; set; }

        public int PayForId { get; set; }

        public int DealId { get; set; }

        [Required]
        [Display(Name = "Card number")]
        [DataType(DataType.CreditCard)]
        public string CardNumber { get; set; } 

        [Required]
        [DataType(DataType.Date)]
        public DateTime ValidBefore { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^[0-9]{3}$")]
        public string CvvCode { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string FormAction { get; set; }
    }
}