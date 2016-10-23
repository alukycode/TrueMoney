namespace TrueMoney.Models.ViewModels
{
    using System;

    using TrueMoney.Models.Basic;

    public class DealIndexViewModel
    {
        public DealModel Deal { get; set; }

        public int CurrentUserId { get; set; }

        //public bool IsCurrentUserOwner { get; set; }
    }
}