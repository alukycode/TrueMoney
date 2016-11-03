namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentPlanId = c.Int(nullable: false),
                        DateOfPayment = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentPlans", t => t.PaymentPlanId, cascadeDelete: true)
                .Index(t => t.PaymentPlanId);
            
            CreateTable(
                "dbo.PaymentPlans",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        DealId = c.Int(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deals", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Deals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OwnerId = c.Int(nullable: false),
                        ResultOfferId = c.Int(),
                        PaymentPlanId = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        CloseDate = c.DateTime(),
                        Description = c.String(),
                        DealPeriod = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DealStatus = c.Int(nullable: false),
                        InterestRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaymentCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.OwnerId, cascadeDelete: true)
                .ForeignKey("dbo.Offers", t => t.ResultOfferId)
                .Index(t => t.OwnerId)
                .Index(t => t.ResultOfferId);
            
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OffererId = c.Int(),
                        DealId = c.Int(nullable: false),
                        InterestRate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreateTime = c.DateTime(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.OffererId)
                .ForeignKey("dbo.Deals", t => t.DealId, cascadeDelete: true)
                .Index(t => t.OffererId)
                .Index(t => t.DealId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        MiddleName = c.String(),
                        PassportId = c.Int(),
                        AspUserId = c.String(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        BankAccountNumber = c.String(nullable: false),
                        Rating = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Passports", t => t.PassportId)
                .Index(t => t.PassportId);
            
            CreateTable(
                "dbo.Passports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Series = c.String(nullable: false),
                        Number = c.String(nullable: false),
                        DateOfIssuing = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentPlanId = c.Int(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Liability = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsPaid = c.Boolean(nullable: false),
                        PaidDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PaymentPlans", t => t.PaymentPlanId, cascadeDelete: true)
                .Index(t => t.PaymentPlanId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BankTransactions", "PaymentPlanId", "dbo.PaymentPlans");
            DropForeignKey("dbo.Payments", "PaymentPlanId", "dbo.PaymentPlans");
            DropForeignKey("dbo.PaymentPlans", "Id", "dbo.Deals");
            DropForeignKey("dbo.Deals", "ResultOfferId", "dbo.Offers");
            DropForeignKey("dbo.Deals", "OwnerId", "dbo.Users");
            DropForeignKey("dbo.Offers", "DealId", "dbo.Deals");
            DropForeignKey("dbo.Users", "PassportId", "dbo.Passports");
            DropForeignKey("dbo.Offers", "OffererId", "dbo.Users");
            DropIndex("dbo.Payments", new[] { "PaymentPlanId" });
            DropIndex("dbo.Users", new[] { "PassportId" });
            DropIndex("dbo.Offers", new[] { "DealId" });
            DropIndex("dbo.Offers", new[] { "OffererId" });
            DropIndex("dbo.Deals", new[] { "ResultOfferId" });
            DropIndex("dbo.Deals", new[] { "OwnerId" });
            DropIndex("dbo.PaymentPlans", new[] { "Id" });
            DropIndex("dbo.BankTransactions", new[] { "PaymentPlanId" });
            DropTable("dbo.Payments");
            DropTable("dbo.Passports");
            DropTable("dbo.Users");
            DropTable("dbo.Offers");
            DropTable("dbo.Deals");
            DropTable("dbo.PaymentPlans");
            DropTable("dbo.BankTransactions");
        }
    }
}
