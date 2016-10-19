using System.Collections.Generic;
using TrueMoney.Models.Basic;

namespace TrueMoney.Models
{
    public class YouActivityViewModel
    {
        public IList<DealModel> Deals { get; set; } = new List<DealModel>();
        public IList<OfferModel> Offers { get; set; } = new List<OfferModel>();
    }
}