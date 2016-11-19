using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace TrueMoney.Models.Deal
{
    public class CreateDealForm
    {
        public decimal Amount { get; set; }

        public int PaymentCount { get; set; }

        public decimal InterestRate { get; set; }

        public int DealPeriod { get; set; }

        public string Description { get; set; }

        public bool IsUserCanCreateDeal { get; set; }

        public int OwnerId { get; set; }
    }
}