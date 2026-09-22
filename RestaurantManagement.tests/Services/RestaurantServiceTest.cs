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
        private Mock<IAddressRepository> _addressRespository;
        private RestaurantService _restaurantService;

        [TestInitialize]
        public void Setup()
        {
            _restaurantRepository = new Mock<IRestaurantRepository>();
            _addressRespository = new Mock<IAddressRepository>();
            _restaurantService = new RestaurantService(_restaurantRepository.Object,_addressRespository.Object);
        }

        [TestMethod]
        public async Task GetRestaurantsAsync_ReturnsActiveRestaurants()
        {
            // Arrange
            var restaurants = new List<Restaurant>
    {
        new Restaurant { RestaurantId = 1, Name = "Active Rest", AddressId = 10, Email = "test1@test.com", PhoneNumber = "123" },
        new Restaurant { RestaurantId = 2, Name = "Another Active", AddressId = 20, Email = "test2@test.com", PhoneNumber = "456" }
    };

            _restaurantRepository
                .Setup(r => r.GetRestaurantsAsync())
                .ReturnsAsync(restaurants);

=            _addressRespository
                .Setup(e => e.GetAddress(It.IsAny<int>()))
                .ReturnsAsync((int addressId) => new Address
                {
                    Street = "Street " + addressId,
                    City = "City",
                    State = "State",
                    Country = "Country",
                    PinCode = "12345",
                    AddressType = AddressType.Work
                });

            // Act
            var result = await _restaurantService.GetRestaurantsAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
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
       
    }
}