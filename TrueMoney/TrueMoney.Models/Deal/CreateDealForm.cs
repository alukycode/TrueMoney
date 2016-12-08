using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using TrueMoney.Common;

namespace TrueMoney.Models.Deal
{
    public class CreateDealForm
    {
        [Display(Name="Необходимое количество денег (р.)")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        [Range(1, 100000, ErrorMessage = "Сумма займа должна быть в пределах от 1 до 100 000 р")]
        public decimal? Amount { get; set; }

        [Display(Name="Желаемая процентная ставка (%)")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        [Range(1, 1000, ErrorMessage = "Процентная ставка должна быть в пределах от 1 до 1000 %")]
        public decimal? InterestRate { get; set; }

        [Display(Name = "Планируемое количество платежей по займу до срока возврата займа")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        [Range(1, 1000, ErrorMessage = "Количество платежей должно быть в пределах от 1 до 1000 раз")]
        public int? PaymentCount { get; set; }

        [Display(Name="На какой срок нужны деньги? (дней)")]
        [Required(ErrorMessage = ErrorMessages.Required)]
        [Range(1, 3650, ErrorMessage = "Минимальный срок - 1 день, максимальный - 10 лет (3650 дней)")]
        public int? DealPeriod { get; set; }

        [Display(Name="С какой целью берется займ?")]
        public string Description { get; set; }

        public bool IsCurrentUserActive { get; set; }

        public bool HaveOpenDeal { get; set; }
    }
}