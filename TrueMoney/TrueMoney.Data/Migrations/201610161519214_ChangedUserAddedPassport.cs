namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedUserAddedPassport : DbMigration
    {
        public override void Up()
        {
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
            
            AddColumn("dbo.Users", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "BankAccountNumber", c => c.String(nullable: false));
            AddColumn("dbo.Users", "Rating", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "Passport_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Users", "AspUserId", c => c.String(nullable: false));
            CreateIndex("dbo.Users", "Passport_Id");
            AddForeignKey("dbo.Users", "Passport_Id", "dbo.Passports", "Id", cascadeDelete: true);
            DropColumn("dbo.Users", "Passport_Series");
            DropColumn("dbo.Users", "Passport_Number");
            DropColumn("dbo.Users", "Passport_DateOfIssuing");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Passport_DateOfIssuing", c => c.DateTime());
            AddColumn("dbo.Users", "Passport_Number", c => c.String());
            AddColumn("dbo.Users", "Passport_Series", c => c.String());
            DropForeignKey("dbo.Users", "Passport_Id", "dbo.Passports");
            DropIndex("dbo.Users", new[] { "Passport_Id" });
            AlterColumn("dbo.Users", "AspUserId", c => c.String());
            DropColumn("dbo.Users", "Passport_Id");
            DropColumn("dbo.Users", "Rating");
            DropColumn("dbo.Users", "BankAccountNumber");
            DropColumn("dbo.Users", "IsActive");
            DropTable("dbo.Passports");
        }
    }
}
