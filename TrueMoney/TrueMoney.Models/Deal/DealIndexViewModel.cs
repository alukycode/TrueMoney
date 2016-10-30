namespace TrueMoney.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using TrueMoney.Models.Basic;

    public class DealIndexViewModel
    {
        public IList<DealModel> Deals { get; set; }

        public int CurrentUserId { get; set; }
    }
}