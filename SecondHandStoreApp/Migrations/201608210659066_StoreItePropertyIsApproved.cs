namespace SecondHandStoreApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StoreItePropertyIsApproved : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreItems", "IsApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StoreItems", "IsApproved");
        }
    }
}
