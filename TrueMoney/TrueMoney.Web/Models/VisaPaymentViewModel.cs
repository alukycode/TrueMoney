namespace TrueMoney.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class VisaPaymentViewModel
    {
        public string PaymentName { get; set; }

        public decimal PaymentCount { get; set; }

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
        [RegularExpression("^[0-0]{3}$")]
        public string CvvCode { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}