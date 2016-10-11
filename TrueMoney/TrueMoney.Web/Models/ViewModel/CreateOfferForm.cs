namespace TrueMoney.Web.Models.ViewModel
{
    using System.ComponentModel.DataAnnotations;

    public class CreateOfferForm
    {
        public int AppId { get; set; }

        [Required]
        [RegularExpression("^[0-9]{1,2}[.][0-9]{1,2}$", ErrorMessage = "Используйте формат XX.XX")]
        [Display(Name = "Процентная ставка", Description = "Должна быть меньше максимальнодопустимой.")]
        public float Rate { get; set; }
    }
}