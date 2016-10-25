namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDealPeriod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Deals", "DealPeriodTicks", c => c.Long(nullable: false));
            DropColumn("dbo.Deals", "DealPeriod");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Deals", "DealPeriod", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.Deals", "DealPeriodTicks");
        }
    }
}
