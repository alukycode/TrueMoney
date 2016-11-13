namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Deals", "PaymentPlanId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Deals", "PaymentPlanId", c => c.Int());
        }
    }
}
