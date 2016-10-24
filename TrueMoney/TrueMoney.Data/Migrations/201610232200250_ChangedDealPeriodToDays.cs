namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDealPeriodToDays : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Deals", "DealPeriod", c => c.Int(nullable: false));
            DropColumn("dbo.Deals", "DealPeriodTicks");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Deals", "DealPeriodTicks", c => c.Long(nullable: false));
            DropColumn("dbo.Deals", "DealPeriod");
        }
    }
}
