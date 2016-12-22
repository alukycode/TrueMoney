using System.Linq;
using TrueMoney.Common.Enums;
using TrueMoney.Data;

namespace TrueMoney.Services.Helpers
{
    public static class GlobalHelper
    {
        public static GlobalInfo Get(int currentUserId)
        {
            var context = new TrueMoneyContext();

            var user = context.Users.Single(x => x.Id == currentUserId);

            var userDeals = context.Deals.Where(x => x.OwnerId == currentUserId).ToList();
            var activeDeal = userDeals.SingleOrDefault(x => x.DealStatus != DealStatus.Closed);

            var approvedUserOffers = context.Offers.Where(x => x.IsApproved && x.Deal.DealStatus != DealStatus.Closed && x.OffererId == currentUserId).ToList();

            return new GlobalInfo
            {
                IsActive = user.IsActive,
                ActiveDealStatus = activeDeal?.DealStatus ?? (userDeals.Any() ? (DealStatus?)DealStatus.Closed : null), // чернуха :D
                ActiveDealBestOfferPercent = activeDeal?.Offers.OrderByDescending(x => x.InterestRate).FirstOrDefault()?.InterestRate,
                ActiveDealOffersCount = activeDeal?.Offers.Count,
                OffersInWaitCount = approvedUserOffers.Count(x => x.Deal.DealStatus == DealStatus.WaitForApprove || x.Deal.DealStatus == DealStatus.WaitForLoan),
                OffersInProgressCount = approvedUserOffers.Count(x => x.Deal.DealStatus == DealStatus.InProgress),
            };
        }
    }

    public class GlobalInfo
    {
        public bool        IsActive                   { get; set; }
        public DealStatus? ActiveDealStatus           { get; set; }
        public decimal?    ActiveDealBestOfferPercent { get; set; }
        public int?        ActiveDealOffersCount      { get; set; }
        public int         OffersInWaitCount          { get; set; }
        public int         OffersInProgressCount      { get; set; }
    }
}
