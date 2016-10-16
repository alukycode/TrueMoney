namespace TrueMoney.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDeal : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.BankTransactions", name: "PaymentPlan_Id", newName: "PaymentPlanId");
            RenameColumn(table: "dbo.Payments", name: "PaymentPlan_Id", newName: "PaymentPlanId");
            RenameColumn(table: "dbo.Offers", name: "Deal_Id", newName: "DealId");
            RenameColumn(table: "dbo.Deals", name: "Owner_Id", newName: "OwnerId");
            RenameColumn(table: "dbo.Offers", name: "Offerer_Id", newName: "OffererId");
            RenameIndex(table: "dbo.BankTransactions", name: "IX_PaymentPlan_Id", newName: "IX_PaymentPlanId");
            RenameIndex(table: "dbo.Deals", name: "IX_Owner_Id", newName: "IX_OwnerId");
            RenameIndex(table: "dbo.Offers", name: "IX_Offerer_Id", newName: "IX_OffererId");
            RenameIndex(table: "dbo.Offers", name: "IX_Deal_Id", newName: "IX_DealId");
            RenameIndex(table: "dbo.Payments", name: "IX_PaymentPlan_Id", newName: "IX_PaymentPlanId");
            AddColumn("dbo.PaymentPlans", "DealId", c => c.Int(nullable: false));
            AddColumn("dbo.Deals", "PaymentPlanId", c => c.Int());
            AddColumn("dbo.Deals", "IsApproved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Deals", "CreateDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Deals", "Description", c => c.String());
            AddColumn("dbo.Deals", "DueDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Deals", "DealStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Deals", "DealStatus");
            DropColumn("dbo.Deals", "DueDate");
            DropColumn("dbo.Deals", "Description");
            DropColumn("dbo.Deals", "CreateDate");
            DropColumn("dbo.Deals", "IsApproved");
            DropColumn("dbo.Deals", "PaymentPlanId");
            DropColumn("dbo.PaymentPlans", "DealId");
            RenameIndex(table: "dbo.Payments", name: "IX_PaymentPlanId", newName: "IX_PaymentPlan_Id");
            RenameIndex(table: "dbo.Offers", name: "IX_DealId", newName: "IX_Deal_Id");
            RenameIndex(table: "dbo.Offers", name: "IX_OffererId", newName: "IX_Offerer_Id");
            RenameIndex(table: "dbo.Deals", name: "IX_OwnerId", newName: "IX_Owner_Id");
            RenameIndex(table: "dbo.BankTransactions", name: "IX_PaymentPlanId", newName: "IX_PaymentPlan_Id");
            RenameColumn(table: "dbo.Offers", name: "OffererId", newName: "Offerer_Id");
            RenameColumn(table: "dbo.Deals", name: "OwnerId", newName: "Owner_Id");
            RenameColumn(table: "dbo.Offers", name: "DealId", newName: "Deal_Id");
            RenameColumn(table: "dbo.Payments", name: "PaymentPlanId", newName: "PaymentPlan_Id");
            RenameColumn(table: "dbo.BankTransactions", name: "PaymentPlanId", newName: "PaymentPlan_Id");
        }
    }
}
