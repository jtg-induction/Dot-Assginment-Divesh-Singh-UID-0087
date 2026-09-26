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

                TotalAmount = 25.00m,
                Status = "Placed",

            };
            // Setup Claim Helper behavior
            _mockClaimHelper.Setup(e => e.GetUserIdFromClaim(It.IsAny<System.Security.Principal.IIdentity>())).ReturnsAsync(mockUserId);


            // Setup Order Service behavior
            _mockOrderService
                .Setup(s => s.AddOrder(requestPayload, mockUserId))
                .ReturnsAsync(mockOrderResponse);

            // 2. ACT
            IHttpActionResult actionResult = await _controller.AddOrder(requestPayload);

            // 3. ASSERT
            var createdResult = actionResult as CreatedNegotiatedContentResult<BaseResponse<OrderResponse>>;

            Assert.IsNotNull(createdResult);
        }
        [TestMethod]
        public async Task GetOrderDetail_WhenOrdersExist_ReturnsOkWithOrdersList()
        {
            // Arrange
            int targetUserId = 1;
            var mockOrders = new List<GetOrderResponse>
        {
            new GetOrderResponse { OrderId = 101, TotalAmount = 45.00m },
            new GetOrderResponse { OrderId = 102, TotalAmount = 20.50m }
        };

            _mockClaimHelper.Setup(e => e.GetUserIdFromClaim(It.IsAny<System.Security.Principal.IIdentity>())).ReturnsAsync(123);

            _mockOrderService
                .Setup(s => s.GetOrder(targetUserId))
                .ReturnsAsync(mockOrders);

            // Act
            var result = await _controller.GetOrder();

            // Assert
            var okResult = result as OkNegotiatedContentResult<BaseResponse<List<GetOrderResponse>>>;
            Assert.IsNotNull(okResult);

        }
        [TestMethod]
        public async Task GetOrderItemDetail_WhenItemsExist_ReturnsOkWithItemsList()
        {
            // Arrange
            int orderId = 101;
            var mockItems = new List<GetOrderItemResponse>
        {
            new GetOrderItemResponse { ItemId = 5001, ItemName = "Burger" },
            new GetOrderItemResponse { ItemId = 5002, ItemName = "Fries" }
        };

            _mockOrderService
                .Setup(s => s.GetOrderItem(orderId,1))
                .ReturnsAsync(mockItems);

            // Act
            var result = await _controller.GetOrderItem(orderId);

            // Assert
            var okResult = result as OkNegotiatedContentResult<BaseResponse<List<GetOrderItemResponse>>>;
            Assert.IsNotNull(okResult);

        }
        [TestMethod]
        public async Task CancelOrder_WhenCalled_ExecutesSuccessfullyAndReturnsOk()
        {
            // Arrange
            int orderId = 101;
            int targetUserId = 1;

            _mockClaimHelper.Setup(e => e.GetUserIdFromClaim(It.IsAny<System.Security.Principal.IIdentity>())).ReturnsAsync(123);


            _mockOrderService
                .Setup(s => s.OrderCancel(orderId, targetUserId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CancelOrder(orderId);

            // Assert
            var okResult = result as OkNegotiatedContentResult<BaseResponse<string>>;
            Assert.IsNotNull(okResult);

        }
        [TestMethod]
        public async Task GetAllOrder_ValidParams_ReturnsOkWithCorrectData()
        {
            // Arrange
            int userId = 42;
            var paginationParams = new OrderRequestForOwner { pageNumber = 1, pageSize = 5 };

            var expectedPaginatedData = new GetPaginatedResponse<GetOrderResponseForOwner>
            {
                order = new List<GetOrderResponseForOwner> { new GetOrderResponseForOwner() },
                pagination = new PaginationMetaData { TotalItems = 1, PageSize = 5, CurrentPage = 1, TotalPages = 1 }
            };

            _mockClaimHelper
                .Setup(c => c.GetUserIdFromClaim(_controller.User.Identity))
                .ReturnsAsync(userId);

            _mockOrderService
                .Setup(s => s.GetAllOrder(paginationParams, userId))
                .ReturnsAsync(expectedPaginatedData);

            // Act
            IHttpActionResult actionResult = await _controller.GetAllOrder(paginationParams);

            // Assert
            var contentResult = actionResult as OkNegotiatedContentResult<BaseResponse<GetPaginatedResponse<GetOrderResponseForOwner>>>;


            Assert.IsNotNull(contentResult.Content);
        }
    }
}
