namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "FirstName", c => c.String(nullable: false));
            AddColumn("dbo.Users", "LastName", c => c.String(nullable: false));
            AddColumn("dbo.Users", "MiddleName", c => c.String());
            AddColumn("dbo.Users", "Passprt_Series", c => c.String());
            AddColumn("dbo.Users", "Passprt_Number", c => c.String());
            AddColumn("dbo.Users", "Passprt_DateOfIssuing", c => c.DateTime());
            DropColumn("dbo.Users", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Name", c => c.String());
            DropColumn("dbo.Users", "Passprt_DateOfIssuing");
            DropColumn("dbo.Users", "Passprt_Number");
            DropColumn("dbo.Users", "Passprt_Series");
            DropColumn("dbo.Users", "MiddleName");
            DropColumn("dbo.Users", "LastName");
            DropColumn("dbo.Users", "FirstName");
        }
    }
}
