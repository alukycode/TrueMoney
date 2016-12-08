using System.ComponentModel.DataAnnotations;

namespace TrueMoney.Models.Offer
{
    using System.Web.Mvc;

    using TrueMoney.Models.Basic;

    public class CreateOfferForm
    {
        [Required]
        [Display(Name = "Процентная ставка (%)")]
        [Range(0, 1000, ErrorMessage = "Значение процента не может быть меньше 0 и больше, чем 1000%")]
        public decimal InterestRate { get; set; }

        [HiddenInput]
        public decimal DealRate { get; set; }

        [HiddenInput]
        public int OffererId { get; set; }

        [HiddenInput]
        public int DealId { get; set; }
    }
}