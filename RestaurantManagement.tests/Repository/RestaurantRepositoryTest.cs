using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Effort;
using System.Data.Common;

namespace RestaurantManagement.tests.Repository
{
    [TestClass]
    public class RestaurantRepositoryTest
    {
        private ApplicationDbContext _context;
        private RestaurantRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            DbConnection connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new ApplicationDbContext(connection);
            _repository = new RestaurantRepository(_context);
        }

        [TestMethod]
        public async Task GetRestaurantsAsync_ReturnsOnlyActiveRestaurants()
        {
            // Seed
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

            _context.Restaurants.Add(new Restaurant
            {
                RestaurantId = 1,
                Name = "Active Rest",
                AddressId = 1,
                Email = "active@test.com",
                PhoneNumber = "9999999999",
                IsActive = true
            });

            _context.Restaurants.Add(new Restaurant
            {
                RestaurantId = 2,
                Name = "Inactive Rest",
                AddressId = 1,
                Email = "inactive@test.com",
                PhoneNumber = "8888888888",
                IsActive = false
            });

            _context.SaveChanges();

            // Act
            var result = await _repository.GetRestaurantsAsync();

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Active Rest", result[0].Name);
        }

        [TestMethod]
        public async Task GetRestaurantsAsync_ReturnsEmptyList_WhenNoActiveRestaurants()
        {
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

            _context.Restaurants.Add(new Restaurant
            {
                RestaurantId = 1,
                Name = "Inactive Rest",
                AddressId = 1,
                Email = "inactive@test.com",
                PhoneNumber = "8888888888",
                IsActive = false
            });

            _context.SaveChanges();

            var result = await _repository.GetRestaurantsAsync();

            Assert.AreEqual(0, result.Count);
        }
    }
}