using System.Data.Entity;
using System.Threading.Tasks;
using TrueMoney.Data.Entities;

namespace TrueMoney.Data
{
    public interface ITrueMoneyContext
    {
        IDbSet<BankTransaction> BankTransactions { get; set; }

        IDbSet<Deal> Deals { get; set; }

        IDbSet<Offer> Offers { get; set; }

        IDbSet<Passport> Passports { get; set; }

        IDbSet<PaymentPlan> PaymentPlans { get; set; }

        IDbSet<Payment> Payments { get; set; }

        IDbSet<User> Users { get; set; }

        Task<int> SaveChangesAsync();
    }
}