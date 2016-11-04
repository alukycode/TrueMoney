using System.Data.Entity.Migrations;

namespace TrueMoney.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<TrueMoneyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            CommandTimeout = 300;
        }

        protected override void Seed(TrueMoneyContext context)
        {
            TrueMoneyDbInitializer.InitializeData(context);
        }
    }
}
