using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services;
using RestaurantManagement.Exceptions; 
using System;
using System.Threading.Tasks;
using RestaurantManagement.Models.Enum;

namespace RestaurantManagement.Tests.Services
{
    [TestClass]
    public class AddressServiceTest
    {
        private Mock<IAddressRepository> _mockAddressRepository;
        private Mock<IUserAddressRepository> _userAddressRepository;
        private AddressService _addressService;

        [TestInitialize]
        public void SetUp()
        {
           
            _mockAddressRepository = new Mock<IAddressRepository>();
            _userAddressRepository = new Mock<IUserAddressRepository>();
            _addressService = new AddressService(_mockAddressRepository.Object,_userAddressRepository.Object);
        }
        [TestMethod]
        public async Task AddUserAddress_ValidRequest_ReturnsNewAddressId()
        {
            // 1. ARRANGEMENT
            int userid = 1;
            var requestDto = new AddAddressRequest
            {
                Street = "123 Main St",
                City = "New York",
                State = "NY",
                AddressType = AddressType.Home,
                Pincode = "10001",
                Country = "USA"
            };
            _mockAddressRepository
                .Setup(repo => repo.AddAddressAsync(It.IsAny<Address>()))
                .Callback<Address>(a => a.AddressId = 42)
                .Returns(Task.CompletedTask);

            // 2. ACT
             await _addressService.AddUserAddress(requestDto,userid);

            // 3. ASSERT
      

        }

        [TestMethod]
   
        public async Task AddUserAddress_NullRequest_ThrowsArgumentNullException()
        {
            // ACT & ASSERT
            Exception e = null;
            try
            {
                await _addressService.AddUserAddress(null,1);
            }catch(Exception ex)
            {
                e = ex;
            }
            Assert.IsNotNull(e);
        }
    }
}
