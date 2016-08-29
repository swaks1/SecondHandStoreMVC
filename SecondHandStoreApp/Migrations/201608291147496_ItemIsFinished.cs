namespace SecondHandStoreApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ItemIsFinished : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreItems", "IsFinished", c => c.Boolean(nullable: false));
            AddColumn("dbo.StoreItems", "heelSize", c => c.Double());
            AlterColumn("dbo.Sellers", "TransactionNum", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sellers", "TransactionNum", c => c.String());
            DropColumn("dbo.StoreItems", "heelSize");
            DropColumn("dbo.StoreItems", "IsFinished");
        }
    }
}
