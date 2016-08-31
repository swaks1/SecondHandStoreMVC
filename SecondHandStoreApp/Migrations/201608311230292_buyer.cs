namespace SecondHandStoreApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class buyer : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.MyUserStoreItems", newName: "StoreItemMyUsers");
            DropPrimaryKey("dbo.StoreItemMyUsers");
            CreateTable(
                "dbo.Deliveries",
                c => new
                    {
                        ID = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        BuyerId = c.Int(),
                        itemId = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MyUsers", t => t.BuyerId)
                .ForeignKey("dbo.StoreItems", t => t.ID)
                .Index(t => t.ID)
                .Index(t => t.BuyerId);
            
            AddColumn("dbo.StoreItems", "orderId", c => c.Int());
            AddPrimaryKey("dbo.StoreItemMyUsers", new[] { "StoreItem_ID", "MyUser_ID" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Deliveries", "ID", "dbo.StoreItems");
            DropForeignKey("dbo.Deliveries", "BuyerId", "dbo.MyUsers");
            DropIndex("dbo.Deliveries", new[] { "BuyerId" });
            DropIndex("dbo.Deliveries", new[] { "ID" });
            DropPrimaryKey("dbo.StoreItemMyUsers");
            DropColumn("dbo.StoreItems", "orderId");
            DropTable("dbo.Deliveries");
            AddPrimaryKey("dbo.StoreItemMyUsers", new[] { "MyUser_ID", "StoreItem_ID" });
            RenameTable(name: "dbo.StoreItemMyUsers", newName: "MyUserStoreItems");
        }
    }
}
