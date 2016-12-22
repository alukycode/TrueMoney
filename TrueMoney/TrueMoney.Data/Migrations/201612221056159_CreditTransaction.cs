namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreditTransaction : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreditTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        RecipientId = c.Int(),
                        SenderId = c.Int(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.RecipientId)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId)
                .ForeignKey("dbo.Deals", t => t.Id, cascadeDelete: true)
                .Index(t => t.Id)
                .Index(t => t.RecipientId)
                .Index(t => t.SenderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CreditTransactions", "Id", "dbo.Deals");
            DropForeignKey("dbo.CreditTransactions", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.CreditTransactions", "RecipientId", "dbo.AspNetUsers");
            DropIndex("dbo.CreditTransactions", new[] { "SenderId" });
            DropIndex("dbo.CreditTransactions", new[] { "RecipientId" });
            DropIndex("dbo.CreditTransactions", new[] { "Id" });
            DropTable("dbo.CreditTransactions");
        }
    }
}
