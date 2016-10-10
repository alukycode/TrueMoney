using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace TrueMoney.Data.Entities
{
    public class User : Entity
    {
        public User()
        {
            Passport = new Passport();
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public Passport Passport { get; set; }

        public string AspUserId { get; set; }
    }

    public class Passport
    {
        public string Series { get; set; }

        public string Number { get; set; }

        public DateTime? DateOfIssuing { get; set; }
    }
}
