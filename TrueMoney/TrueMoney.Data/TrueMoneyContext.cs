using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using TrueMoney.Data.Entities;

namespace TrueMoney.Data
{
    public class TrueMoneyContext : IdentityDbContext<User, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>, ITrueMoneyContext
    {
        public TrueMoneyContext() : base("DefaultConnection")
        {
            Database.CommandTimeout = 300;

            Database.SetInitializer(new TrueMoneyDbInitializer());
        }

        public IDbSet<Passport> Passports { get; set; }

        public IDbSet<Deal> Deals { get; set; }

        public IDbSet<Offer> Offers { get; set; }

        public IDbSet<PaymentPlan> PaymentPlans { get; set; }

        public IDbSet<Payment> Payments { get; set; }

        public IDbSet<BankTransaction> BankTransactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<IdentityUser>().ToTable("Users", "dbo");
            //modelBuilder.Entity<User>        ().ToTable("Users", "dbo");
        }
    }
}
