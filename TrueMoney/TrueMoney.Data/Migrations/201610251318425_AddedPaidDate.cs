namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPaidDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payments", "PaidDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Payments", "PaidDate");
        }
    }
}
