using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestaurantManagement.Controllers;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Services.Interface; 
using RestaurantManagement.Helper;
using RestaurantManagement.Constants;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Http.Results;
using System.Web.Http;

namespace RestaurantManagement.Tests.Controllers
{
    [TestClass]
    public class OrderControllerTest
    {
        private Mock<IOrderService> _mockOrderService;
        private Mock<IClaimHelper> _mockClaimHelper;
        private OrderController _controller;

        [TestInitialize]
        public void SetUp()
        {
            _mockOrderService = new Mock<IOrderService>();
            _mockClaimHelper = new Mock<IClaimHelper>();

            _controller = new OrderController(_mockOrderService.Object, _mockClaimHelper.Object);
          
        }

        [TestMethod]
        public async Task AddOrder_ValidPayload_ReturnsCreatedWithSuccessResponse()
        {
            // 1. ARRANGEMENT
            int mockUserId = 42;
            var requestPayload = new AddOrderRequest
            {
                AddressId = 10,
                ItemAndQuantity = new Dictionary<int, int> { { 101, 2 } }
            };

            var mockOrderResponse = new OrderResponse
            {
                OrderId = 777,
                RestaurantId = 5,
                TotalAmount = 25.00m,
                Status = "Placed",
                Address = "123 Main St"
            };
            // Setup Claim Helper behavior
            _mockClaimHelper.Setup(e => e.GetUserIdFromClaim(It.IsAny<System.Security.Principal.IIdentity>())).ReturnsAsync(123);


            // Setup Order Service behavior
            _mockOrderService
                .Setup(s => s.AddOrder(requestPayload, mockUserId))
                .ReturnsAsync(mockOrderResponse);

            // 2. ACT
            IHttpActionResult actionResult = await _controller.AddOrder(requestPayload);

            // 3. ASSERT
            var createdResult = actionResult as CreatedNegotiatedContentResult<BaseResponse<OrderResponse>>;

            Assert.IsNotNull(createdResult, "The controller action should return an HTTP 201 Created result.");
        }
    }
}
