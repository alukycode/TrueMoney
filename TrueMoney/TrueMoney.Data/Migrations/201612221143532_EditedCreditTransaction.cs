namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditedCreditTransaction : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CreditTransactions", "DateOfPayment", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CreditTransactions", "DateOfPayment");
        }
    }
}
