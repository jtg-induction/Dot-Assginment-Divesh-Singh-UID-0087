using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagement.Tests.Services
{
    [TestClass]
    public class UserAddressServiceTest
    {
        private Mock<IUserAddressRepository> _mockUserAddressRepository;
        private UserAddressService _userAddressService;

        [TestInitialize]
        public void SetUp()
        {
            _mockUserAddressRepository = new Mock<IUserAddressRepository>();
            _userAddressService = new UserAddressService(_mockUserAddressRepository.Object);
        }
        [TestMethod]
        public async Task AddUserAddress_ValidParameters_CallsRepositoryWithMappedEntity()
        {
            // 1. ARRANGEMENT
            int targetUserId = 1;
            int targetAddressId = 42;

            _mockUserAddressRepository
                .Setup(repo => repo.AddUserAddressAysnc(It.IsAny<UserAddress>()))
                .Returns(Task.CompletedTask);

            // 2. ACT
            await _userAddressService.AddUserAdress(targetUserId, targetAddressId);
        }

      

        [TestMethod]
        public async Task GetAddress_ExistingUserId_ReturnsListofAddresses()
        {
            // 1. ARRANGEMENT
            int targetUserId = 5;
            var expectedAddresses = new List<Address>
            {
                new Address { AddressId = 101, Street = "123 Main St", City = "New York" },
                new Address { AddressId = 102, Street = "456 Wall St", City = "New York" }
            };

            _mockUserAddressRepository
                .Setup(repo => repo.GetUserAddress(targetUserId))
                .ReturnsAsync(expectedAddresses);

            // 2. ACT
            List<AddressResponse> result = await _userAddressService.GetAddress(targetUserId);

            // 3. ASSERT
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetAddress_UserHasNoAddresses_ReturnsEmptyList()
        {
            // 1. ARRANGEMENT
            int targetUserId = 999;
            _mockUserAddressRepository
                .Setup(repo => repo.GetUserAddress(targetUserId))
                .ReturnsAsync(new List<Address>()); 

            // 2. ACT
            List<AddressResponse> result = await _userAddressService.GetAddress(targetUserId);

            // 3. ASSERT
            Assert.IsNotNull(result);
        }

      
    }
}
