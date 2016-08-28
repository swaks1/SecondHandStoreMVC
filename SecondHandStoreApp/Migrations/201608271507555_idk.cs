namespace SecondHandStoreApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class idk : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.StoreItems", "ItemName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.StoreItems", "ItemName", c => c.String());
        }
    }
}
