namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PassportEntityChanges : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Passports", "GiveOrganization", c => c.String());
            DropColumn("dbo.Passports", "Series");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Passports", "Series", c => c.String(nullable: false));
            DropColumn("dbo.Passports", "GiveOrganization");
        }
    }
}
