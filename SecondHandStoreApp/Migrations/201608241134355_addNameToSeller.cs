namespace SecondHandStoreApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNameToSeller : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sellers", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sellers", "Name");
        }
    }
}
