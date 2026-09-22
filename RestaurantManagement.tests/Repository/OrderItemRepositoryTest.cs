using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Repository;
using RestaurantManagement.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace RestaurantManagement.tests.Repository
{
    [TestClass]
    public class OrderItemRepositoryTest
    {
        private ApplicationDbContext _context;
        private OrderItemRepository _OrderItemRepository;

        [TestInitialize]
        public void Setup()
        {
            DbConnection connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new ApplicationDbContext(connection);

            _OrderItemRepository = new OrderItemRepository(_context);
        }
        [TestMethod]
        public void add()
        {
            // 1. Setup required parent entities (Foreign Keys)
            _context.Addresses.Add(new Address
            {
                AddressId = 1,
                Street = "123 Main Road",
                City = "Mumbai",
                State = "Maharashtra",
                PinCode = "400001",
                Country = "India",
                AddressType = AddressType.Home
            });
            var testUser = new Models.Entity.User
            {
                UserId = 1,
                Name = "Jane Doe",
                Password = "PasswordSecure123!",
                Email = "janedoe@example.com",
                BirthDate = new DateTime(1995, 8, 24),
                PhoneNumber = "555-019-2834",
                Role = UserRole.Customer,
                Balance = 5000m,
                IsActive = true
            };
            _context.Users.Add(testUser);

            _context.Restaurants.Add(new Restaurant
            {
                RestaurantId = 1,
                Name = "Active Rest",
                AddressId = 1,
                Email = "active@test.com",
                PhoneNumber = "9999999999",
                IsActive = true
            });

            // 2. Setup the Order entity
            _context.Orders.Add(new Order
            {
                OrderId = 1001,
                UserId = 1,
                RestaurantId = 1,
                TotalAmount = 45.97m,
                Status = OrderStatus.Placed,
                Address = "123 Main Street, Suite 400, New York, NY 10001"
            });

            // 3. Setup OrderItems for the order
            List<OrderItem> order = new List<OrderItem>();
            order.Add(
                new OrderItem
                {
                OrderItemId = 5001,
                OrderId = 1001,
                ItemId = 101,
                ItemName = "Bacon Cheeseburger",
                Price = 15.99m,
                Quantity = 2
            });

            _context.SaveChanges();
            _OrderItemRepository.AddOrderItem(order);

        }
        [TestMethod]
        public async Task GetOrderItem_WhenOrderHasItems_ReturnsMatchingItemsList()
        {
            // Arrange - Seed test data matching OrderId = 1001
            _context.Addresses.Add(new Address
            {
                AddressId = 1,
                Street = "123 Main Road",
                City = "Mumbai",
                State = "Maharashtra",
                PinCode = "400001",
                Country = "India",
                AddressType = AddressType.Home
            });
            var testUser = new Models.Entity.User
            {
                UserId = 1,
                Name = "Jane Doe",
                Password = "PasswordSecure123!",
                Email = "janedoe@example.com",
                BirthDate = new DateTime(1995, 8, 24),
                PhoneNumber = "555-019-2834",
                Role = UserRole.Customer,
                Balance = 5000m,
                IsActive = true
            };
            _context.Users.Add(testUser);

            _context.Restaurants.Add(new Restaurant
            {
                RestaurantId = 1,
                Name = "Active Rest",
                AddressId = 1,
                Email = "active@test.com",
                PhoneNumber = "9999999999",
                IsActive = true
            });
            _context.Orders.Add(new Order
            {
                OrderId = 1001,
                UserId = 1,
                RestaurantId = 1,
                TotalAmount = 45.97m,
                Status = OrderStatus.Placed,
                Address = "123 Main Street, Suite 400, New York, NY 10001"
            });
            _context.OrderItems.Add(new OrderItem
            {
                OrderItemId = 5001,
                OrderId = 1001,
                ItemId = 101,
                ItemName = "Bacon Cheeseburger",
                Price = 15.99m,
                Quantity = 2
            });
            _context.OrderItems.Add(new OrderItem
            {
                OrderItemId = 5002,
                OrderId = 1001,
                ItemId = 102,
                ItemName = "Large Truffle Fries",
                Price = 13.99m,
                Quantity = 1
            });
          
            _context.SaveChanges();

            // Act
            var result = await _OrderItemRepository.GetOrderItem(1001);

            // Assert
            Assert.IsNotNull(result);
           
        }

    }
}
