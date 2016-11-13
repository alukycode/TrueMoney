namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cascadeDeleteForPaymentPlan : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PaymentPlans", "Id", "dbo.Deals");
            AddForeignKey("dbo.PaymentPlans", "Id", "dbo.Deals", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PaymentPlans", "Id", "dbo.Deals");
            AddForeignKey("dbo.PaymentPlans", "Id", "dbo.Deals", "Id");
        }
    }
}
