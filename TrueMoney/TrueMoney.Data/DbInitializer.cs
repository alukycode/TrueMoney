using System.Collections.Generic;
using System.Data.Entity;
using TrueMoney.Data.Entities;

namespace TrueMoney.Data
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<TrueMoneyContext>
    {
        protected override void Seed(TrueMoneyContext context)
        {
            List<User> users = new List<User>
            {
                new User { Name = "Саша" },
                new User { Name = "Антон" },
                new User { Name = "Дима" }
            };

            foreach (var item in users)
            {
                context.Users.Add(item);
            }

            base.Seed(context);

            context.SaveChanges();
        }
    }
}
