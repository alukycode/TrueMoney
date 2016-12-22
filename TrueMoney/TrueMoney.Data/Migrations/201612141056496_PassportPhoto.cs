namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PassportPhoto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Passports", "ImagePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Passports", "ImagePath");
        }
    }
}
