namespace TrueMoney.Web.Models
{
    using System.Collections.Generic;

    using TrueMoney.Infrastructure.Entities;

    public class YouActivityViewModel
    {
        public IList<Deal> Deals { get; set; } = new List<Deal>();
        public IList<Offer> Offers { get; set; } = new List<Offer>();
        public IList<Loan> Loans { get; set; } = new List<Loan>();
    }
}