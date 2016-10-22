using System.ComponentModel.DataAnnotations;

namespace TrueMoney.Models
{
    using System.Web.Mvc;

    public class CreateOfferForm
    {
        public int DealId { get; set; }

        [Required]
        //[RegularExpression("^[0-9]{1,2}[.][0-9]{1,2}$", ErrorMessage = "Используйте формат XX.XX")]
        [Display(Name = "Процентная ставка", Description = "Должна быть меньше максимальнодопустимой.")]
        public decimal Rate { get; set; } //пока пусть лучше int

        [HiddenInput]
        public decimal DealRate { get; set; }

        [HiddenInput]
        public bool IsUserCanCreateOffer { get; set; }
    }
}