using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Repository;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.tests.Repository
{
    [TestClass]
    public class OrderRepositoryTest
    {
        private ApplicationDbContext _context;
        private OrderRepository _orderRepository;

        [TestInitialize]
        public void Setup()
        {
            DbConnection connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new ApplicationDbContext(connection);

            _orderRepository = new OrderRepository(_context);
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
            var testUser = new User
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
            var testOrder = new Order
            {
                OrderId = 1001,
                UserId = 1,               
                RestaurantId = 1,        
                TotalAmount = 45.97m,     
                Status = OrderStatus.Placed,
                Address = "123 Main Street, Suite 400, New York, NY 10001" // Satisfies Min/Max Address Length
            };

            //// 3. Setup OrderItems for the order
            //var orderItem1 = new OrderItem
            //{
            //    OrderItemId = 5001,
            //    OrderId = 1001,          
            //    ItemId = 101,             
            //    ItemName = "Bacon Cheeseburger", 
            //    Price = 15.99m,          
            //    Quantity = 2              
            //};

            //var orderItem2 = new OrderItem
            //{
            //    OrderItemId = 5002,
            //    OrderId = 1001,           
            //    ItemId = 102,
            //    ItemName = "Large Truffle Fries",
            //    Price = 13.99m,
            //    Quantity = 1
            //};


          
            //_context.OrderItems.Add(orderItem1);
            //_context.OrderItems.Add(orderItem2);

            _context.SaveChanges();
            _orderRepository.PlacedOrder(testOrder);
           

        }
    }
}
