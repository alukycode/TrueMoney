namespace TrueMoney.Models.ViewModels
{
    using System.Collections.Generic;

    using TrueMoney.Models.Basic;

    public class YourActivityViewModel
    {
        public IList<DealModel> Deals { get; set; } = new List<DealModel>();

        public IList<OfferModel> Offers { get; set; } = new List<OfferModel>();

        public bool IsCurrentUserActive { get; set; }
    }
}