namespace TrueMoney.Models.Deal
{
    using System.Collections.Generic;
    using System.Linq;

    using TrueMoney.Models.Basic;

    public class DealIndexViewModel
    {
        public IList<DealModel> Deals { get; set; }

        public bool UserCanCreateDeal { get; set; }

        public HashSet<int> DealIdsWithOfferFromCurrentUser { get; set; } 
    }
}