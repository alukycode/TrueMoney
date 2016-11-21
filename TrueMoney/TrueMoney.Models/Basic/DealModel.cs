using System.ComponentModel.DataAnnotations;

namespace TrueMoney.Models.Basic
{
    using System;
    using System.Collections.Generic;
    using Common.Enums;

    public class DealModel
    {
        public int Id { get; set; }

        [Display(Name="Процентная ставка")]
        public decimal InterestRate { get; set; }

        [Display(Name="Дата создания")]
        public DateTime CreateDate { get; set; }

        [Display(Name="Дата закрытия")]
        public DateTime? CloseDate { get; set; }

        [Display(Name = "Необходимая сумма денег")]
        public decimal Amount { get; set; }

        [Display(Name = "На какой срок (дней)")]
        public int DealPeriod { get; set; }

        [Display(Name = "Цель займа")]
        public string Description { get; set; }

        public DealStatus DealStatus { get; set; }

        public int OwnerId { get; set; }

        public string OwnerFullName { get; set; }
    }
}
