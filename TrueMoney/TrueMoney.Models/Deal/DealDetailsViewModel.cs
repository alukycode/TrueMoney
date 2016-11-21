using TrueMoney.Common.Enums;

namespace TrueMoney.Models.Deal
{
    using System.Collections.Generic;
    using System.Linq;

    using TrueMoney.Models.Basic;

    public class DealDetailsViewModel
    {
        public DealModel Deal { get; set; }

        public bool IsCurrentUserOwner => CurrentUserId == Deal.OwnerId;

        public bool IsCurrentUserActive { get; set; }

        public int CurrentUserId { get; set; }

        public bool IsCurrentUserLender
        {
            get
            {
                return Offers != null && Offers.Any() && Offers.Any(x => x.OffererId == CurrentUserId);
            }
        }

        public OfferModel CurrentUserOffer
        {
            get
            {
                return Offers.FirstOrDefault(x => x.OffererId == CurrentUserId);
            }
        }

        public IList<OfferModel> Offers { get; set; }

        public PaymentPlanModel PaymentPlanModel { get; set; }

        public IList<PaymentModel> Payments { get; set; }

        public decimal ExtraMoney { get; set; }
    }
}