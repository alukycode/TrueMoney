namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MamkuEbal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentPlans", "CreateTime", c => c.DateTime(nullable: false));
            DropColumn("dbo.PaymentPlans", "CreateDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentPlans", "CreateDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.PaymentPlans", "CreateTime");
        }
    }
}
