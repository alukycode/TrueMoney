namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
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
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId, cascadeDelete: true)
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
                .ForeignKey("dbo.AspNetUsers", t => t.OffererId)
                .ForeignKey("dbo.Deals", t => t.DealId, cascadeDelete: true)
                .Index(t => t.OffererId)
                .Index(t => t.DealId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(nullable: false),
                        MiddleName = c.String(),
                        PassportId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        BankAccountNumber = c.String(nullable: false),
                        Rating = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Passports", t => t.PassportId)
                .Index(t => t.PassportId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
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
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
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
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.BankTransactions", "PaymentPlanId", "dbo.PaymentPlans");
            DropForeignKey("dbo.Payments", "PaymentPlanId", "dbo.PaymentPlans");
            DropForeignKey("dbo.PaymentPlans", "Id", "dbo.Deals");
            DropForeignKey("dbo.Deals", "ResultOfferId", "dbo.Offers");
            DropForeignKey("dbo.Deals", "OwnerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Offers", "DealId", "dbo.Deals");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "PassportId", "dbo.Passports");
            DropForeignKey("dbo.Offers", "OffererId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Payments", new[] { "PaymentPlanId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "PassportId" });
            DropIndex("dbo.Offers", new[] { "DealId" });
            DropIndex("dbo.Offers", new[] { "OffererId" });
            DropIndex("dbo.Deals", new[] { "ResultOfferId" });
            DropIndex("dbo.Deals", new[] { "OwnerId" });
            DropIndex("dbo.PaymentPlans", new[] { "Id" });
            DropIndex("dbo.BankTransactions", new[] { "PaymentPlanId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Payments");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.Passports");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Offers");
            DropTable("dbo.Deals");
            DropTable("dbo.PaymentPlans");
            DropTable("dbo.BankTransactions");
        }
    }
}
