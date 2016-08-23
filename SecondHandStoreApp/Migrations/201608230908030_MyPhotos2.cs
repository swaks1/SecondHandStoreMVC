namespace SecondHandStoreApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MyPhotos2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.MyImages", name: "StoreItem_ID", newName: "StoreItemId");
            RenameIndex(table: "dbo.MyImages", name: "IX_StoreItem_ID", newName: "IX_StoreItemId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.MyImages", name: "IX_StoreItemId", newName: "IX_StoreItem_ID");
            RenameColumn(table: "dbo.MyImages", name: "StoreItemId", newName: "StoreItem_ID");
        }
    }
}
