using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueMoney.Data.Entities;

namespace TrueMoney.Data
{
    public class TrueMoneyContext : DbContext, ITrueMoneyContext
    {
        public TrueMoneyContext() : base("DefaultConnection")
        {
            Database.SetInitializer(new TrueMoneyDbInitializer());
        }

        public IDbSet<User> Users{ get; set; }

        public IDbSet<Passport> Passports { get; set; }

        //public IDbSet<Deal> Deals{ get; set; }

        //public IDbSet<Offer> Offers { get; set; }

        //public IDbSet<PaymentPlan> PaymentPlans { get; set; }

        //public IDbSet<Payment> Payments { get; set; }

        //public IDbSet<BankTransaction> BankTransactions { get; set; }
    }
}
