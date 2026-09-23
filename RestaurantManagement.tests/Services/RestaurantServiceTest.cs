using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.repository;
using RestaurantManagement.Repository;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagement.tests.Services
{
    [TestClass]
    public class RestaurantServiceTest
    {
        private Mock<IRestaurantRepository> _restaurantRepository;
        private Mock<IAddressRepository> _addressRespository;
        private Mock<IUserRepository> _userRepository;
        private Mock<IRestaurantOwnerRepository> _restaurantownerRepository;
        private RestaurantService _restaurantService;

        [TestInitialize]
        public void Setup()
        {
            _restaurantRepository = new Mock<IRestaurantRepository>();
            _addressRespository = new Mock<IAddressRepository>();
            _userRepository = new Mock<IUserRepository>();
            _restaurantownerRepository = new Mock<IRestaurantOwnerRepository>();
            _restaurantService = new RestaurantService(_restaurantRepository.Object,_userRepository.Object,_addressRespository.Object,_restaurantownerRepository.Object);
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

            _addressRespository
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
        [TestMethod]
        public async Task AddRestaurant_ValidRequest_SuccessfullyCreatesRestaurantAndOwners()
        {
            // Arrange
            var request = new AddRestaurantRequest
            {
                Email = "new@restaurant.com",
                PhoneNumber = "1234567890",
                UserEmail = new List<string> { "owner1@test.com", "owner2@test.com" },
                Street = "Main St",
                City = "New York",
                State = "NY",
                Pincode = "10001",
                AddressType = AddressType.Work,
                Name = "Tasty Bites"
            };

            // Setup repository mocks to pass initial validation rules
            _restaurantRepository.Setup(r => r.EmailExixts(request.Email)).ReturnsAsync(false);
            _restaurantRepository.Setup(r => r.PhoneNumberExixts(request.PhoneNumber)).ReturnsAsync(false);
            _userRepository.Setup(r => r.EmailExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

            _userRepository.Setup(r => r.GetUserAsync("owner1@test.com")).ReturnsAsync(new User { UserId = 101 });
            _userRepository.Setup(r => r.GetUserAsync("owner2@test.com")).ReturnsAsync(new User { UserId = 102 });

            // Act
            await _restaurantService.AddRestaurant(request);

            // Assert
        }
        [TestMethod]
        public async Task AddRestaurantOwner_ValidRequest_SuccessfullyAppendsOwners()
        {
            // Arrange
            var request = new AddRestaurantOwnerRequest
            {
                RestaurantEmail = "active@restaurant.com",
                UserEmail = new List<string> { "newowner@test.com" }
            };
            var activeRestaurant = new Restaurant { RestaurantId = 5, Name = "Active Cafe" };

            _restaurantRepository.Setup(r => r.RestaurantIsActive(request.RestaurantEmail)).ReturnsAsync(activeRestaurant);
            _userRepository.Setup(r => r.EmailExistsAsync("newowner@test.com")).ReturnsAsync(true);
            _userRepository.Setup(r => r.GetUserAsync("newowner@test.com")).ReturnsAsync(new User { UserId = 88 });

            // Act
            await _restaurantService.AddRestaurantowner(request);

            // Assert
         
        }

    }
}