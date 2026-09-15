using System.Data.Entity.Migrations;

namespace RestaurantManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RefreshTokens",
                c => new
                    {
                        TokenId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Token = c.String(nullable: false),
                        IsRevoked = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        UpdatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.TokenId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            AddColumn("dbo.Users", "Balance_Updated_At", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Addresses", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Addresses", "UpdatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.MenuItems", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.MenuItems", "UpdatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Restaurants", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Restaurants", "UpdatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.OrderItems", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.OrderItems", "UpdatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Orders", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Orders", "UpdatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Users", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Users", "UpdatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.RestaurantOwners", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.RestaurantOwners", "UpdatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            DropColumn("dbo.Addresses", "Country");
            DropColumn("dbo.Users", "BalanceUpdatedAt");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "BalanceUpdatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.Addresses", "Country", c => c.String(nullable: false));
            DropForeignKey("dbo.RefreshTokens", "UserId", "dbo.Users");
            DropIndex("dbo.RefreshTokens", new[] { "UserId" });
            AlterColumn("dbo.RestaurantOwners", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RestaurantOwners", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OrderItems", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OrderItems", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Restaurants", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Restaurants", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MenuItems", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MenuItems", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Addresses", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Addresses", "CreatedAt", c => c.DateTime(nullable: false));
            DropColumn("dbo.Users", "Balance_Updated_At");
            DropTable("dbo.RefreshTokens");
        }
    }
}
