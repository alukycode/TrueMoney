namespace TrueMoney.Infrastructure.Entities
{
    using System;
    using System.Collections.Generic;

    public class User : Entity
    {
        public User()
        {
            Passport = new Passport();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public Passport Passport { get; set; }
    }

    public class Passport
    {
        public string Series { get; set; }

        public string Number { get; set; }

        public DateTime DateOfIssuing { get; set; }
    }
}
