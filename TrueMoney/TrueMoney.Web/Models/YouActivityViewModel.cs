namespace TrueMoney.Web.Models
{
    using System.Collections.Generic;

    using TrueMoney.Infrastructure.Entities;

    public class YouActivityViewModel
    {
        public IList<MoneyApplication> MoneyApplications { get; set; } = new List<MoneyApplication>();
        public IList<Offer> Offers { get; set; } = new List<Offer>();
        public IList<Loan> Loans { get; set; } = new List<Loan>();
    }
}