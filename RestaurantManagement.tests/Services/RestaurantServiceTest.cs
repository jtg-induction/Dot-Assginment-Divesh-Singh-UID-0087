using Moq;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Repository;
using RestaurantManagement.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantManagement.Repository.Interface;

namespace RestaurantManagement.tests.Services
{
    [TestClass]
    public class RestaurantServiceTest
    {
        private Mock<IRestaurantRepository> _restaurantRepository;
        private RestaurantService _restaurantService;

        [TestInitialize]
        public void Setup()
        {
            _restaurantRepository = new Mock<IRestaurantRepository>();
            _restaurantService = new RestaurantService(_restaurantRepository.Object);
        }

        [TestMethod]
        public async Task GetRestaurantsAsync_ReturnsActiveRestaurants()
        {
            // Arrange
            var restaurants = new List<Restaurant>
            {
                new Restaurant { RestaurantId = 1, Name = "Active Rest", IsActive = true },
                new Restaurant { RestaurantId = 2, Name = "Another Active", IsActive = true }
            };

            _restaurantRepository
                .Setup(r => r.GetRestaurantsAsync())
                .ReturnsAsync(restaurants);

            // Act
            var result = await _restaurantService.GetRestaurantsAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Active Rest", result[0].Name);
            Assert.AreEqual("Another Active", result[1].Name);
        }

        [TestMethod]
        public async Task GetRestaurantsAsync_ReturnsEmptyList_WhenNoActiveRestaurants()
        {
            _restaurantRepository
                .Setup(r => r.GetRestaurantsAsync())
                .ReturnsAsync(new List<Restaurant>());

            var result = await _restaurantService.GetRestaurantsAsync();

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetRestaurantsAsync_CallsRepository()
        {
            _restaurantRepository
                .Setup(r => r.GetRestaurantsAsync())
                .ReturnsAsync(new List<Restaurant>());

            await _restaurantService.GetRestaurantsAsync();

            _restaurantRepository.Verify(r => r.GetRestaurantsAsync(), Times.Once);
        }
    }
}