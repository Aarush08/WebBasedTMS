namespace WebBasedTMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DelteeMoreFeilds1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "MobileNumber");
            DropColumn("dbo.AspNetUsers", "Name");
            DropColumn("dbo.AspNetUsers", "Role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Role", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Name", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.AspNetUsers", "MobileNumber", c => c.String(nullable: false, maxLength: 13));
        }
    }
}
