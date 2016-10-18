namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class entitiesUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Deals", "CloseDate", c => c.DateTime());
            DropColumn("dbo.Deals", "IsApproved");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Deals", "IsApproved", c => c.Boolean(nullable: false));
            DropColumn("dbo.Deals", "CloseDate");
        }
    }
}
