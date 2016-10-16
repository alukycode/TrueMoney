using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueMoney.Data.Entities
{
    public class Offer : Entity
    {
        public int? OffererId { get; set; }

        public User Offerer { get; set; }

        public int DealId { get; set; }

        [Required]
        public Deal Deal { get; set; }

        public int Percent { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsApproved { get; set; }
    }
}
