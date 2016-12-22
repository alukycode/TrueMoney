using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueMoney.Data.Entities
{
    public class Passport : Entity
    {
        [Required]
        public string Number { get; set; }

        public DateTime? DateOfIssuing { get; set; }

        public string GiveOrganization { get; set; }

        public string ImagePath { get; set; }
    }
}
