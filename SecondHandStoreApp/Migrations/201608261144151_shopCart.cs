namespace SecondHandStoreApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class shopCart : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MyUserStoreItems",
                c => new
                    {
                        MyUser_ID = c.Int(nullable: false),
                        StoreItem_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.MyUser_ID, t.StoreItem_ID })
                .ForeignKey("dbo.MyUsers", t => t.MyUser_ID, cascadeDelete: true)
                .ForeignKey("dbo.StoreItems", t => t.StoreItem_ID, cascadeDelete: true)
                .Index(t => t.MyUser_ID)
                .Index(t => t.StoreItem_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MyUserStoreItems", "StoreItem_ID", "dbo.StoreItems");
            DropForeignKey("dbo.MyUserStoreItems", "MyUser_ID", "dbo.MyUsers");
            DropIndex("dbo.MyUserStoreItems", new[] { "StoreItem_ID" });
            DropIndex("dbo.MyUserStoreItems", new[] { "MyUser_ID" });
            DropTable("dbo.MyUserStoreItems");
        }
    }
}
