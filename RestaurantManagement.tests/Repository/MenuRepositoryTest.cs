using Castle.DynamicProxy.Generators;
using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Repository;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RestaurantManagement.tests.Repository
{
    [TestClass]
    public class MenuRepositoryTest
    {
        private ApplicationDbContext _context;
        private MenuRepository _menuRepository;

        [TestInitialize]
        public void Setup()
        {
            DbConnection connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new ApplicationDbContext(connection);
     
            _menuRepository = new MenuRepository(_context);
        }
        [TestMethod]
        public async Task GetMenuItem()
        {
            // 1. Address 
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

            // 2. Restaurant
            _context.Restaurants.Add(new Restaurant
            {
                RestaurantId = 1,
                Name = "Test Restaurant",
                AddressId = 1,
                Email = "test@example.com",
                PhoneNumber = "9999999999"
            });

            // 3. MenuItems
            _context.MenuItems.AddRange(new List<MenuItem>
    {
        new MenuItem { ItemId = 1, RestaurantId = 1, DishName = "Pizza", Price = 100m, AvailableQuantity = 10 },
        new MenuItem { ItemId = 2, RestaurantId = 1, DishName = "Burger", Price = 80m,  AvailableQuantity = 5 }
    });

            _context.SaveChanges();

            var result = await _menuRepository.GetMenuItem(1);
            Assert.AreEqual(2, result.Count());
        }
          
        }
    }



