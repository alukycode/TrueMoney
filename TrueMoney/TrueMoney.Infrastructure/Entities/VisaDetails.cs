namespace TrueMoney.Infrastructure.Entities
{
    using System;

    public class VisaDetails
    {
        public string CardNumber { get; set; }
        public DateTime ValidBefore { get; set; }
        public string Name { get; set; }
        public string CvvCode { get; set; }
    }
}