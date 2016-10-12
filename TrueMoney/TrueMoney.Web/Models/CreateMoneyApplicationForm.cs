namespace TrueMoney.Web.Models
{
    public class CreateMoneyApplicationForm
    {
         public float Count { get; set; }
         public float Rate { get; set; }
         public int DayCount { get; set; }
         public string Description { get; set; }
    }
}