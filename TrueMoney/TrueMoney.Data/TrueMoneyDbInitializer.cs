using System.Collections.Generic;
using System.Data.Entity;
using TrueMoney.Data.Entities;

namespace TrueMoney.Data
{
    public class TrueMoneyDbInitializer : DropCreateDatabaseIfModelChanges<TrueMoneyContext>
    {
        protected override void Seed(TrueMoneyContext context)
        {
            List<User> users = new List<User>
            {
                new User
                {
                    FirstName = "Саша",
                    LastName = "Черногребель",
                    AspUserId = "test",
                    BankAccountNumber = "test",
                },
                new User
                {
                    FirstName = "Антон",
                    LastName = "Лукьянов",
                    AspUserId = "test",
                    BankAccountNumber = "test",
                },
                new User
                {
                    FirstName = "Дима",
                    LastName = "Артюх",
                    AspUserId = "test",
                    BankAccountNumber = "test",
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
