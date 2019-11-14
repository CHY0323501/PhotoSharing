namespace PhotoSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        CommentID = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false, maxLength: 100),
                        Body = c.String(nullable: false, maxLength: 400),
                        UserName = c.String(nullable: false),
                        PhotoID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CommentID)
                .ForeignKey("dbo.Photos", t => t.PhotoID, cascadeDelete: true)
                .Index(t => t.PhotoID);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        PhotoID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 150),
                        PhotoFile = c.Binary(nullable: false),
                        ImageMimeType = c.String(),
                        Description = c.String(nullable: false, maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                        UserName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.PhotoID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "PhotoID", "dbo.Photos");
            DropIndex("dbo.Comments", new[] { "PhotoID" });
            DropTable("dbo.Photos");
            DropTable("dbo.Comments");
        }
    }
}
