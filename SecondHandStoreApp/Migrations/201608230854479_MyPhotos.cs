namespace SecondHandStoreApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MyPhotos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MyImages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Image = c.String(),
                        StoreItem_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.StoreItems", t => t.StoreItem_ID)
                .Index(t => t.StoreItem_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MyImages", "StoreItem_ID", "dbo.StoreItems");
            DropIndex("dbo.MyImages", new[] { "StoreItem_ID" });
            DropTable("dbo.MyImages");
        }
    }
}
