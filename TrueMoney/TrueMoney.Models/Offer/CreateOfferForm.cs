using System.ComponentModel.DataAnnotations;

namespace TrueMoney.Models.Offer
{
    using System.Web.Mvc;

    using TrueMoney.Models.Basic;

    public class CreateOfferForm
    {
        [Required]
        //[RegularExpression("^[0-9]{1,2}[.][0-9]{1,2}$", ErrorMessage = "Используйте формат XX.XX")]
        [Display(Name = "Процентная ставка", Description = "Должна быть меньше максимальнодопустимой.")]
        public decimal InterestRate { get; set; } //пока пусть лучше int

        [HiddenInput]
        public decimal DealRate { get; set; }

        [HiddenInput]
        public int OffererId { get; set; }

        [HiddenInput]
        public int DealId { get; set; }
    }
}