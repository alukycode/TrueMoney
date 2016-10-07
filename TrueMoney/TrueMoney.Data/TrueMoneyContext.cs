using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueMoney.Data.Entities;

namespace TrueMoney.Data
{
    public class TrueMoneyContext : DbContext
    {
        public TrueMoneyContext() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users{ get; set; }
    }
}
