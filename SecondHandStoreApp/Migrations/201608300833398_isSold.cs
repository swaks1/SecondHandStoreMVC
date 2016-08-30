namespace SecondHandStoreApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isSold : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StoreItems", "isSold", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StoreItems", "isSold");
        }
    }
}
