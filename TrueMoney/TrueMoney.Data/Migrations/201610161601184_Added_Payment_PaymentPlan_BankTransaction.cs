namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Added_Payment_PaymentPlan_BankTransaction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentPlan_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentPlans", t => t.PaymentPlan_Id, cascadeDelete: true)
                .Index(t => t.PaymentPlan_Id);
            
            CreateTable(
                "dbo.PaymentPlans",
                c => new
                    {
                        Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deals", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentPlan_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentPlans", t => t.PaymentPlan_Id, cascadeDelete: true)
                .Index(t => t.PaymentPlan_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BankTransactions", "PaymentPlan_Id", "dbo.PaymentPlans");
            DropForeignKey("dbo.Payments", "PaymentPlan_Id", "dbo.PaymentPlans");
            DropForeignKey("dbo.PaymentPlans", "Id", "dbo.Deals");
            DropIndex("dbo.Payments", new[] { "PaymentPlan_Id" });
            DropIndex("dbo.PaymentPlans", new[] { "Id" });
            DropIndex("dbo.BankTransactions", new[] { "PaymentPlan_Id" });
            DropTable("dbo.Payments");
            DropTable("dbo.PaymentPlans");
            DropTable("dbo.BankTransactions");
        }
    }
}
