using System;
using System.Collections.Generic;
using System.Linq;
using TrueMoney.Common.Enums;
using TrueMoney.Models.Basic;

namespace TrueMoney.Models.User
{
    public class UserProfileModel
    {
        public PassportModel Passport { get; set; }

        public UserModel User { get; set; }

        public IList<DealModel> Deals { get; set; } = new List<DealModel>();

        public DealInfoModel ActiveDealInfo {get;set;}

        public IList<OfferModel> Offers { get; set; } = new List<OfferModel>();

        public bool IsCurrentUserActive { get; set; }

        public bool HasOpenDeal
        {
            get
            {
                return Deals.Any(x => x.DealStatus != DealStatus.Closed);
            }
        }
    }

    public class DealInfoModel
    {
        public int OffersCount { get; set; }
        
        public decimal BestOfferPercent { get; set; }
    }
}