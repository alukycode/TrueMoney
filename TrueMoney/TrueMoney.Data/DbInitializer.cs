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
                new User
                {
                    FirstName = "Саша",
                    LastName = "Черногребель",
                },
                new User
                {
                    FirstName = "Антон",
                    LastName = "Лукьянов"
                },
                new User
                {
                    FirstName = "Дима",
                    LastName = "Артюх"
                }
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
