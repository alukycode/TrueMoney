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
            Database.SetInitializer(new TrueMoneyDbInitializer());
        }

        public DbSet<User> Users{ get; set; }

        public DbSet<Passport> Passports { get; set; }

        public DbSet<Deal> Deals{ get; set; }

        public DbSet<Offer> Offers { get; set; }

        public DbSet<PaymentPlan> PaymentPlans { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<BankTransaction> BankTransactions { get; set; }
    }
}
