namespace TrueMoney.Models.ViewModels
{
    using System;
    using System.Collections.Generic;

    using TrueMoney.Models.Basic;

    public class DealDetailsViewModel
    {
        public DealModel Deal { get; set; }

        public bool IsCurrentUserBorrower { get; set; }
        
        public int CurrentUserId { get; set; }
        
        public bool IsCurrentUserLender { get; set; }

        public bool ShowOffers => IsCurrentUserBorrower;

        public OfferModel CurrentUserOffer { get; set; }

        public bool IsCanDeleteOffer => IsCurrentUserBorrower && !Deal.IsClosed && !Deal.IsInProgress;
        
        public bool IsMustLoanMoney => IsCurrentUserLender && !Deal.IsWaitForLoan;
    }
}