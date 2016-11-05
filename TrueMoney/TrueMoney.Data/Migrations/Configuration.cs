using System.Data.Entity.Migrations;

namespace TrueMoney.Data.Migrations
{
    // how to delete all migrations and start from scratch 
    // http://stackoverflow.com/questions/11679385/reset-entity-framework-migrations
    // this file will be regenerated

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
