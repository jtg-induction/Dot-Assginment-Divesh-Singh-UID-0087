namespace RestaurantManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inital1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Addresses", "Country", c => c.String(nullable: false));
            AddColumn("dbo.Users", "BalanceUpdatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Addresses", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Addresses", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MenuItems", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.MenuItems", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Restaurants", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Restaurants", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OrderItems", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.OrderItems", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Users", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RefreshTokens", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RefreshTokens", "UpdatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RestaurantOwners", "CreatedAt", c => c.DateTime(nullable: false));
            AlterColumn("dbo.RestaurantOwners", "UpdatedAt", c => c.DateTime(nullable: false));
            DropColumn("dbo.Users", "Balance_Updated_At");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Balance_Updated_At", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.RestaurantOwners", "UpdatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.RestaurantOwners", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.RefreshTokens", "UpdatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.RefreshTokens", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Users", "UpdatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Users", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Orders", "UpdatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Orders", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.OrderItems", "UpdatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.OrderItems", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Restaurants", "UpdatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Restaurants", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.MenuItems", "UpdatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.MenuItems", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Addresses", "UpdatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Addresses", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            DropColumn("dbo.Users", "BalanceUpdatedAt");
            DropColumn("dbo.Addresses", "Country");
        }
    }
}
