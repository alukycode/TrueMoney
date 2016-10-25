namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<TrueMoney.Data.TrueMoneyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(TrueMoneyContext context)
        {
            if (!context.Users.Any())
            {
                TrueMoneyDbInitializer.InitializeData(context);
            }
        }
    }
}
