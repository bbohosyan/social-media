namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class someMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserAccounts", "Email", c => c.String());
            AddColumn("dbo.UserAccounts", "UserAccount_UserID", c => c.Int());
            AddColumn("dbo.UserAccounts", "UserAccount_UserID1", c => c.Int());
            AddColumn("dbo.UserAccounts", "UserAccount_UserID2", c => c.Int());
            CreateIndex("dbo.UserAccounts", "UserAccount_UserID");
            CreateIndex("dbo.UserAccounts", "UserAccount_UserID1");
            CreateIndex("dbo.UserAccounts", "UserAccount_UserID2");
            AddForeignKey("dbo.UserAccounts", "UserAccount_UserID", "dbo.UserAccounts", "UserID");
            AddForeignKey("dbo.UserAccounts", "UserAccount_UserID1", "dbo.UserAccounts", "UserID");
            AddForeignKey("dbo.UserAccounts", "UserAccount_UserID2", "dbo.UserAccounts", "UserID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAccounts", "UserAccount_UserID2", "dbo.UserAccounts");
            DropForeignKey("dbo.UserAccounts", "UserAccount_UserID1", "dbo.UserAccounts");
            DropForeignKey("dbo.UserAccounts", "UserAccount_UserID", "dbo.UserAccounts");
            DropIndex("dbo.UserAccounts", new[] { "UserAccount_UserID2" });
            DropIndex("dbo.UserAccounts", new[] { "UserAccount_UserID1" });
            DropIndex("dbo.UserAccounts", new[] { "UserAccount_UserID" });
            DropColumn("dbo.UserAccounts", "UserAccount_UserID2");
            DropColumn("dbo.UserAccounts", "UserAccount_UserID1");
            DropColumn("dbo.UserAccounts", "UserAccount_UserID");
            DropColumn("dbo.UserAccounts", "Email");
        }
    }
}
