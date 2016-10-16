namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDealAndOffer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Deals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Owner_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Owner_Id, cascadeDelete: true)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.Offers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deal_Id = c.Int(nullable: false),
                        Offerer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Deals", t => t.Deal_Id, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.Offerer_Id)
                .Index(t => t.Deal_Id)
                .Index(t => t.Offerer_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Deals", "Owner_Id", "dbo.Users");
            DropForeignKey("dbo.Offers", "Offerer_Id", "dbo.Users");
            DropForeignKey("dbo.Offers", "Deal_Id", "dbo.Deals");
            DropIndex("dbo.Offers", new[] { "Offerer_Id" });
            DropIndex("dbo.Offers", new[] { "Deal_Id" });
            DropIndex("dbo.Deals", new[] { "Owner_Id" });
            DropTable("dbo.Offers");
            DropTable("dbo.Deals");
        }
    }
}
