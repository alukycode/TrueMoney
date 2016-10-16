namespace TrueMoney.Infrastructure.Entities
{
    using System;
    using System.Collections.Generic;

    public class User : Entity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public Passport Passport { get; set; }
        
        public bool IsActive { get; set; }

        // review: зачем нам Equals? сравнивать юзеров
        public override bool Equals(object o)
        {
            var otherUser = o as User;
            return otherUser != null && this.Id == otherUser.Id;
        }

        public string AspUserId { get; set; }

        public BankAccount BankAccount { get; set; }
        public bool IsHaveOpenDealOrLoan { get; set; }

        public int Rating { get; set; } = 0;
    }

    // review: для каждого класса свой файл
    public class Passport
    {
        public string Series { get; set; }

        public string Number { get; set; }

        public DateTime DateOfIssuing { get; set; }

        public string GiveOrganisation { get; set; }
    }
}
