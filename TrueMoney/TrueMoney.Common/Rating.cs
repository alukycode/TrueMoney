namespace TrueMoney.Common
{
    public static class Rating
    {
        public static readonly int StartRating = 0;

        public static readonly int AfterDeactivation = -1;

        public static readonly int AfterActivation = 1;

        public static readonly int SuccessFinishDeal = 5;

        public static readonly int SuccessPayments = 2;

        public static readonly int RevertFinalOffer = -2;

        public static readonly int DelayPayment = -2;
    }
}