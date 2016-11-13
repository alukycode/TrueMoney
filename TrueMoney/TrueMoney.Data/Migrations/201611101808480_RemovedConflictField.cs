namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedConflictField : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PaymentPlans", "DealId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PaymentPlans", "DealId", c => c.Int(nullable: false));
        }
    }
}
