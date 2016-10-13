namespace TrueMoney.Web.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateMoneyApplicationForm
    {
         public float Count { get; set; }
         public int PaymentCount { get; set; }
         public float Rate { get; set; }
         public int DayCount { get; set; }
         public string Description { get; set; }
    }
}