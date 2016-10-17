namespace TrueMoney.Infrastructure.Entities
{
    using System;

    public class Passport
    {
        public string Series { get; set; }

        public string Number { get; set; }

        public DateTime DateOfIssuing { get; set; }

        public string GiveOrganisation { get; set; }
    }
}