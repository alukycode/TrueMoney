namespace TrueMoney.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TrueMoney.Models.Basic;

    public class DealIndexViewModel
    {
        public IList<DealModel> Deals { get; set; }

        public int CurrentUserId { get; set; }

        public bool CurrentUserHasActiveDeals
        {
            get
            {
                return Deals.Count > 0 && Deals.Any(x => x.OwnerId == CurrentUserId);
            }
        }
    }
}