namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class passportRenamed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Passport_Series", c => c.String());
            AddColumn("dbo.Users", "Passport_Number", c => c.String());
            AddColumn("dbo.Users", "Passport_DateOfIssuing", c => c.DateTime());
            DropColumn("dbo.Users", "Passprt_Series");
            DropColumn("dbo.Users", "Passprt_Number");
            DropColumn("dbo.Users", "Passprt_DateOfIssuing");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Passprt_DateOfIssuing", c => c.DateTime());
            AddColumn("dbo.Users", "Passprt_Number", c => c.String());
            AddColumn("dbo.Users", "Passprt_Series", c => c.String());
            DropColumn("dbo.Users", "Passport_DateOfIssuing");
            DropColumn("dbo.Users", "Passport_Number");
            DropColumn("dbo.Users", "Passport_Series");
        }
    }
}
