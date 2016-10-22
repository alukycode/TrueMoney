namespace TrueMoney.Models
{
    public class CreateDealForm
    {
        public decimal Amount { get; set; }
        public int PaymentCount { get; set; }
        public decimal Rate { get; set; }
        public int DayCount { get; set; }
        public string Description { get; set; }
        public bool IsUserCanCreateDeal { get; set; }
    }
}