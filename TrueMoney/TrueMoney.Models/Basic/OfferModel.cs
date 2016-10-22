namespace TrueMoney.Models.Basic
{
    public class OfferModel
    {
        public int LenderId { get; set; }

        public bool IsCurrentUserLender { get; set; }

        public bool IsWaitForApprove { get; set; }

        public float Rate { get; set; }

        public int Id { get; set; }

        public string FullName { get; set; }

        public int DealId { get; set; }

        public bool IsClosed { get; set; }

        public bool IsApproved { get; set; }
    }
}
