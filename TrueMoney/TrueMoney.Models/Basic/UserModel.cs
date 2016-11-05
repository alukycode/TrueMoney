using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueMoney.Models.Basic
{
    public class UserModel
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public bool IsActive { get; set; }

        public string BankAccountNumber { get; set; }

        public int Rating { get; set; }

        public int? PassportId { get; set; }
    }
}
