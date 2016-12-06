namespace TrueMoney.Models.Admin
{
    using System.Collections.Generic;

    using TrueMoney.Models.Basic;

    public class DealListViewModel
    {
        public IList<DealModel> Deals { get; set; }

        public IList<PaymentPlanModel> PaymentPlans { get; set; }

        public IList<PaymentModel> Payments { get; set; }

        public IList<BankTransactionModel> BankTransactions { get; set; }

        public IList<OfferModel> Offers { get; set; }
    }
}