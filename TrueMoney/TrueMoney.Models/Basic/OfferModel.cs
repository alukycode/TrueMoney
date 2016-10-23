namespace TrueMoney.Models.Basic
{
    public class OfferModel
    {
        public UserModel Lender { get; set; }

        public bool IsCurrentUserLender { get; set; }

        public bool IsWaitForApprove { get; set; }

        public decimal Rate { get; set; }

        public int Id { get; set; }

        public string FullName { get; set; }

        public int DealId { get; set; }

        public bool IsClosed { get; set; }

        public bool IsApproved { get; set; }
    }
}
