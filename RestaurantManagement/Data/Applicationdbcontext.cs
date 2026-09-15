using RestaurantManagement.Models.Entity;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace RestaurantManagement.Data
{
    /// <summary>
    /// Represents the Entity Framework database context for the restaurant management application.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("name=DefaultConnection")
        {
        }

        /// <summary>
        /// Initializes a new instance using the configured default database connection.
        /// </summary>
        public ApplicationDbContext(DbConnection existingConnection)
       : base(existingConnection, contextOwnsConnection: true)
        {
        }

        /// <summary>Gets or sets the users in the application.</summary>
        public DbSet<User> Users { get; set; }

        /// <summary>Gets or sets the restaurants in the application.</summary>
        public DbSet<Restaurant> Restaurants { get; set; }

        /// <summary>Gets or sets the restaurant owners in the application.</summary>
        public DbSet<RestaurantOwner> RestaurantOwners { get; set; }

        /// <summary>Gets or sets the menu items in the application.</summary>
        public DbSet<MenuItem> MenuItems { get; set; }

        /// <summary>Gets or sets the orders in the application.</summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>Gets or sets the items belonging to orders.</summary>
        public DbSet<OrderItem> OrderItems { get; set; }

        /// <summary>Gets or sets the addresses in the application.</summary>
        public DbSet<Address> Addresses { get; set; }

        /// <summary>Gets or sets the user-address relationships.</summary>
        public DbSet<UserAddress> UserAddresses { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        /// <summary>
        /// Configures the database model used by this context.
        /// </summary>
        /// <param name="modelBuilder">The model builder used to configure entities.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

