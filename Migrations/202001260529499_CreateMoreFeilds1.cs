namespace WebBasedTMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateMoreFeilds1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "MobileNumber", c => c.String(nullable: false, maxLength: 13));
            AddColumn("dbo.AspNetUsers", "Name", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.AspNetUsers", "Role", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Role");
            DropColumn("dbo.AspNetUsers", "Name");
            DropColumn("dbo.AspNetUsers", "MobileNumber");
        }
    }
}
