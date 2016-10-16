namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedFieldsChangedPassport : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "Passport_Id", "dbo.Passports");
            DropIndex("dbo.Users", new[] { "Passport_Id" });
            RenameColumn(table: "dbo.Users", name: "Passport_Id", newName: "PassportId");
            AddColumn("dbo.BankTransactions", "DateOfPayment", c => c.DateTime(nullable: false));
            AddColumn("dbo.BankTransactions", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Deals", "InterestRate", c => c.Int(nullable: false));
            AddColumn("dbo.Offers", "InterestRate", c => c.Int(nullable: false));
            AddColumn("dbo.Payments", "IsPaid", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Users", "PassportId", c => c.Int());
            CreateIndex("dbo.Users", "PassportId");
            AddForeignKey("dbo.Users", "PassportId", "dbo.Passports", "Id");
            DropColumn("dbo.Offers", "Percent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Offers", "Percent", c => c.Int(nullable: false));
            DropForeignKey("dbo.Users", "PassportId", "dbo.Passports");
            DropIndex("dbo.Users", new[] { "PassportId" });
            AlterColumn("dbo.Users", "PassportId", c => c.Int(nullable: false));
            DropColumn("dbo.Payments", "IsPaid");
            DropColumn("dbo.Offers", "InterestRate");
            DropColumn("dbo.Deals", "InterestRate");
            DropColumn("dbo.BankTransactions", "Amount");
            DropColumn("dbo.BankTransactions", "DateOfPayment");
            RenameColumn(table: "dbo.Users", name: "PassportId", newName: "Passport_Id");
            CreateIndex("dbo.Users", "Passport_Id");
            AddForeignKey("dbo.Users", "Passport_Id", "dbo.Passports", "Id", cascadeDelete: true);
        }
    }
}
