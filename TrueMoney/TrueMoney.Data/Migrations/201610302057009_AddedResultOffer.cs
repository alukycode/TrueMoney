namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedResultOffer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Deals", "ResultOfferId", c => c.Int());
            CreateIndex("dbo.Deals", "ResultOfferId");
            AddForeignKey("dbo.Deals", "ResultOfferId", "dbo.Offers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Deals", "ResultOfferId", "dbo.Offers");
            DropIndex("dbo.Deals", new[] { "ResultOfferId" });
            DropColumn("dbo.Deals", "ResultOfferId");
        }
    }
}
