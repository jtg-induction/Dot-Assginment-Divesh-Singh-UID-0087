using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestaurantManagement.Controllers;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Services.Interface;
using RestaurantManagement.Helper;
using RestaurantManagement.Constants;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Http;
using RestaurantManagement.Models.Enum;

namespace RestaurantManagement.Tests.Controllers
{
    [TestClass]
    public class AddressControllerTest
    {
        private Mock<IAddressService> _mockAddressService;
        private Mock<IUserAddressService> _mockUserAddressService;
        private Mock<IClaimHelper> _mockClaimHelper;
        private AddressController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockAddressService = new Mock<IAddressService>();
            _mockUserAddressService = new Mock<IUserAddressService>();
            _mockClaimHelper = new Mock<IClaimHelper>();

            _controller = new AddressController(
                _mockAddressService.Object,
                _mockUserAddressService.Object,
                _mockClaimHelper.Object);
          

           }

     

        [TestMethod]
        public async Task AddAddress_ValidPayload_ReturnsOkWithSuccessEnvelop()
        {
            // 1. ARRANGEMENT
            int mockUserId = 12;
            int mockGeneratedAddressId = 404;
            var requestPayload = new AddAddressRequest
            {
                Street = "123 Main St",
                City = "New York",
                State = "NY",
                Pincode = "10001",
                Country = "USA",
                AddressType = AddressType.Home
            };

            _mockClaimHelper.Setup(e => e.GetUserIdFromClaim(It.IsAny<System.Security.Principal.IIdentity>())).ReturnsAsync(123);


            _mockAddressService
                .Setup(s => s.AddUserAddress(requestPayload))
                .ReturnsAsync(mockGeneratedAddressId);

            _mockUserAddressService
                .Setup(s => s.AddUserAdress(mockUserId, mockGeneratedAddressId))
                .Returns(Task.CompletedTask);

            // 2. ACT
            IHttpActionResult actionResult = await _controller.AddAddress(requestPayload);

            // 3. ASSERT
            var okResult = actionResult as OkNegotiatedContentResult<BaseResponse<string>>;

            Assert.IsNotNull(okResult);
        }
        [TestMethod]
        public async Task GetUserAddress_WhenUserHasAddresses_ReturnsOkWithData()
        {
            // Arrange
            int targetUserId = 123;
            var mockAddresses = new List<AddressResponse>
        {
            new AddressResponse { AddressId = 1, Street = "123 Main Road", City = "Mumbai" },
            new AddressResponse { AddressId = 2, Street = "456 Park Lane", City = "Delhi" }
        };

            _mockClaimHelper.Setup(e => e.GetUserIdFromClaim(It.IsAny<System.Security.Principal.IIdentity>())).ReturnsAsync(123);

            _mockUserAddressService
                .Setup(s => s.GetAddress(targetUserId))
                .ReturnsAsync(mockAddresses);

            // Act
            var result = await _controller.GetUserAddress();

            // Assert
            Assert.IsNotNull(result);

            var okResult = result as OkNegotiatedContentResult<BaseResponse<List<AddressResponse>>>;

            Assert.IsNotNull(okResult);
           
        }




    }
}
