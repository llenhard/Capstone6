namespace Capstone6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Assigned = c.String(maxLength: 128),
                        Due = c.DateTime(),
                        Desc = c.String(),
                        Complete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.Assigned)
                .Index(t => t.Assigned);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Name);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "Assigned", "dbo.Users");
            DropIndex("dbo.Tasks", new[] { "Assigned" });
            DropTable("dbo.Users");
            DropTable("dbo.Tasks");
        }
    }
}
