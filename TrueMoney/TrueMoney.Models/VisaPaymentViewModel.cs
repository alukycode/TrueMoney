using System;
using System.ComponentModel.DataAnnotations;
using TrueMoney.Common;

namespace TrueMoney.Models
{
    public class VisaPaymentViewModel
    {
        [Required]
        [Display(Name = "Сумма платежа")]
        public decimal PaymentCount { get; set; }

        public bool CanSetPaymentCount { get; set; }

        public int DealId { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [Display(Name = "Номер карты")]
        [RegularExpression("^[0-9]{16}$", ErrorMessage = "Номер карты состоит из 16 цифр.")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [RegularExpression("^[0-9]{2}[/][0-9]{2}$", ErrorMessage = "Формат должен быть **/**")]
        [Display(Name = "Срок действия")]
        public string ValidBefore { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [Display(Name = "Держатель карты")]
        public string Name { get; set; }

        [Required(ErrorMessage = ErrorMessages.Required)]
        [RegularExpression("^[0-9]{3}$", ErrorMessage = "Формат должен быть ***")]
        [Display(Name = "Секретный код карты CVV")]
        public string CvvCode { get; set; }

        public string FormAction { get; set; }

        public string DealOwnerName { get; set; }

        public string OffererFullName { get; set; }
    }
}