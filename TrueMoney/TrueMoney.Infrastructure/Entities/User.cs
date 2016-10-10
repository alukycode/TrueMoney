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
        
        public bool IsActive { get; set; }

        public override bool Equals(object o)
        {
            var otherUser = o as User;
            return otherUser != null && this.Id == otherUser.Id;
        }
    }

    public class Passport
    {
        public string Series { get; set; }

        public string Number { get; set; }

        public DateTime DateOfIssuing { get; set; }

        public string GiveOrganisation { get; set; }
    }
}
