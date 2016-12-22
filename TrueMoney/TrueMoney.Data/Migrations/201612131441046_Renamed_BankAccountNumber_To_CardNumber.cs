namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Renamed_BankAccountNumber_To_CardNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CardNumber", c => c.String(nullable: false));
            DropColumn("dbo.AspNetUsers", "BankAccountNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "BankAccountNumber", c => c.String(nullable: false));
            DropColumn("dbo.AspNetUsers", "CardNumber");
        }
    }
}
