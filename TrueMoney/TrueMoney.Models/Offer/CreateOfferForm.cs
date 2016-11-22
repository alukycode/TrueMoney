using System.ComponentModel.DataAnnotations;

namespace TrueMoney.Models.Offer
{
    using System.Web.Mvc;

    using TrueMoney.Models.Basic;

    public class CreateOfferForm
    {
        [Required]
        [Display(Name = "Процентная ставка")]
        public decimal InterestRate { get; set; }

        [HiddenInput]
        public decimal DealRate { get; set; }

        [HiddenInput]
        public int OffererId { get; set; }

        [HiddenInput]
        public int DealId { get; set; }
    }
}