namespace PhotoSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addblogurl : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Photos", "PhotoFile", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Photos", "PhotoFile", c => c.Binary(nullable: false));
        }
    }
}
