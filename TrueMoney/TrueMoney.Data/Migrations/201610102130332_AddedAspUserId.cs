namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedAspUserId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "AspUserId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "AspUserId");
        }
    }
}
