using TrueMoney.Common.Enums;

namespace TrueMoney.Models.Basic
{
    public class OfferModel
    {
        public int OffererId { get; set; }

        public string OffererFullName { get; set; }

        public bool IsCurrentUserLender { get; set; }

        public bool IsWaitForApprove { get; set; }

        public decimal InterestRate { get; set; }

        public int Id { get; set; }

        public int DealId { get; set; }

        public bool IsApproved { get; set; }

        public int Rating { get; set; }

        public DealStatus DealStatus { get; set; }

        public decimal DealAmount { get; set; }
    }
}
