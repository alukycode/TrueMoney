using System.ComponentModel.DataAnnotations;

namespace TrueMoney.Models
{
    public class CreateOfferForm
    {
        public int DealId { get; set; }

        [Required]
        //[RegularExpression("^[0-9]{1,2}[.][0-9]{1,2}$", ErrorMessage = "Используйте формат XX.XX")]
        [Display(Name = "Процентная ставка", Description = "Должна быть меньше максимальнодопустимой.")]
        public int Rate { get; set; } //пока пусть лучше int
    }
}