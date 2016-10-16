using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace TrueMoney.Data.Entities
{
    public class User : Entity
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public Passport Passport { get; set; }

        [Required]
        public string AspUserId { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public string BankAccountNumber { get; set; }

        public int Rating { get; set; }

        public List<Deal> Deals { get; set; }

        public List<Offer> Offers{ get; set; }
    }
}
