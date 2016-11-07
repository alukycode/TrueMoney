﻿namespace TrueMoney.Models.User
{
    using System.Collections.Generic;
    using System.Linq;

    using TrueMoney.Common.Enums;
    using TrueMoney.Models.Basic;

    public class ProfileViewModel
    {
        public IList<DealModel> Deals { get; set; } = new List<DealModel>();

        public IList<OfferModel> Offers { get; set; } = new List<OfferModel>();

        public bool IsCurrentUserActive { get; set; }

        public bool IsHaveOpenDealOrLoan
        {
            get
            {
                return Deals.Any(x => x.DealStatus != DealStatus.Closed);
            }
        }
    }
}