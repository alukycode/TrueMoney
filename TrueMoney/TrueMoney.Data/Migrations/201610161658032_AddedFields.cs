namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentPlans", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Deals", "DealPeriod", c => c.Time(nullable: false, precision: 7));
            AddColumn("dbo.Deals", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Offers", "Percent", c => c.Int(nullable: false));
            AddColumn("dbo.Offers", "CreateTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Offers", "IsApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Payments", "DueDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Payments", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Payments", "Liability", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Deals", "DueDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Deals", "DueDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Payments", "Liability");
            DropColumn("dbo.Payments", "Amount");
            DropColumn("dbo.Payments", "DueDate");
            DropColumn("dbo.Offers", "IsApproved");
            DropColumn("dbo.Offers", "CreateTime");
            DropColumn("dbo.Offers", "Percent");
            DropColumn("dbo.Deals", "Amount");
            DropColumn("dbo.Deals", "DealPeriod");
            DropColumn("dbo.PaymentPlans", "CreateDate");
        }
    }
}
