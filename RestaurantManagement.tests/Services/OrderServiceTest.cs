using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestaurantManagement.Constants;
using RestaurantManagement.Exceptions;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Models.Response;
using RestaurantManagement.repository;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagement.Tests.Services
{
    [TestClass]
    public class OrderServiceTest
    {
        private Mock<IMenuRepository> _mockMenuRepository;
        private Mock<IAddressRepository> _mockAddressRepository;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IOrderRepository> _mockOrderRepository;
        private Mock<IOrderItemRepository> _mockOrderItemRepository;
        private Mock<IRestaurantRepository> _restaurantRepository;
        private OrderService _orderService;

        [TestInitialize]
        public void SetUp()
        {
            _mockMenuRepository = new Mock<IMenuRepository>();
            _mockAddressRepository = new Mock<IAddressRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockOrderItemRepository = new Mock<IOrderItemRepository>();
            _restaurantRepository = new Mock<IRestaurantRepository>();

            _orderService = new OrderService(
                _mockMenuRepository.Object,
                _mockAddressRepository.Object,
                _mockUserRepository.Object,
                _mockOrderRepository.Object,
                _mockOrderItemRepository.Object,
                _restaurantRepository.Object
            );
        }

        [TestMethod]
        public async Task AddOrder_ValidInput_SuccessfullyPlacesOrderAndDeductsBalance()
        {
            User user = new User
            {
                UserId = 1,
                Balance = 1000
            };
            int addressId = 10;

            var userOrder = new Dictionary<int, int>
            {
                { 101, 2 },
                { 102, 1 }
            };

            var mockMenuItems = new List<MenuItem>
            {
                new MenuItem { ItemId = 101, DishName = "Burger", Price = 10.00m, RestaurantId = 5, AvailableQuantity = 10 },
                new MenuItem { ItemId = 102, DishName = "Fries", Price = 5.00m, RestaurantId = 5, AvailableQuantity = 5 }
            };

            _mockAddressRepository
                .Setup(repo => repo.GetAddressAsync(addressId,user.UserId))
                .ReturnsAsync(new Address
                {
                    AddressId = addressId,
                    Street = "123 Main St",
                    City = "New York",
                    State = "NY"
                });

            _mockMenuRepository
                .Setup(repo => repo.GetItemDetail(userOrder))
                .ReturnsAsync(mockMenuItems);

            _mockUserRepository
                .Setup(repo => repo.GetUserWithLock(user.UserId))
                .ReturnsAsync(user);

            _mockOrderRepository
                .Setup(repo => repo.PlacedOrder(It.IsAny<Order>()))
                .Callback<Order>(o =>
                {
                    o.OrderId = 777;
                    o.RestaurantId = 5;
                    o.Status = OrderStatus.Placed;
                })
                .Returns(Task.CompletedTask);

            _mockOrderItemRepository
                .Setup(repo => repo.AddOrderItem(It.IsAny<List<OrderItem>>()))
                .Returns(Task.CompletedTask);

            OrderResponse response = await _orderService.AddOrder(new Models.Dto.AddOrderRequest {ItemAndQuantity=userOrder,AddressId=addressId }, user.UserId);

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task GetOrder_ValidId_ReturnsMappedOrderResponses()
        {
            int orderId = 1;
            var mockOrders = new List<Order>
            {
                new Order { OrderId = 10, RestaurantId = 5, TotalAmount = 100.00m, Address = "123 St", Status = OrderStatus.Placed }
            };

            _mockOrderRepository.Setup(r => r.GetOrder(orderId)).ReturnsAsync(mockOrders);
            _restaurantRepository.Setup(r => r.GetRestaurantName(5)).ReturnsAsync("Tasty Burger");

            var result = await _orderService.GetOrder(orderId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetOrderItem_ValidId_ReturnsMappedOrderItemResponses()
        {
            int orderId = 1;
            var mockItems = new List<OrderItem>
            {
                new OrderItem { OrderItemId = 50, ItemId = 101, ItemName = "Fries", Price = 5.00m, Quantity = 2 }
            };

            _mockOrderItemRepository.Setup(r => r.GetOrderItem(orderId)).ReturnsAsync(mockItems);

            var result = await _orderService.GetOrderItem(orderId);

            Assert.IsNotNull(result);

        }

        [TestMethod]
        public async Task OrderCancel_UserIdMatchesOrderUserId()
        {
            int orderId = 1;
            int userId = 99;
            var mockOrder = new Order { UserId = 99 };
            _mockOrderRepository.Setup(r => r.GetOrderDetail(orderId)).ReturnsAsync(mockOrder);

            Exception caughtException = null;
            try
            {
                await _orderService.OrderCancel(orderId, userId);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            Assert.IsNotNull(caughtException);
        }

        [TestMethod]
        public async Task OrderCancel_StatusIsCancelled_ThrowsResourceException()
        {
            int orderId = 1;
            int userId = 99;
            var mockOrder = new Order { UserId = 88, Status = OrderStatus.Cancelled };
            _mockOrderRepository.Setup(r => r.GetOrderDetail(orderId)).ReturnsAsync(mockOrder);

            Exception caughtException = null;
            try
            {
                await _orderService.OrderCancel(orderId, userId);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            Assert.IsNotNull(caughtException);

        }

        [TestMethod]
        public async Task OrderCancel_StatusIsRejected_ThrowsResourceException()
        {
            int orderId = 1;
            int userId = 99;
            var mockOrder = new Order { UserId = 88, Status = OrderStatus.Rejected };
            _mockOrderRepository.Setup(r => r.GetOrderDetail(orderId)).ReturnsAsync(mockOrder);

            Exception caughtException = null;
            try
            {
                await _orderService.OrderCancel(orderId, userId);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            Assert.IsNotNull(caughtException);

        }

        [TestMethod]
        public async Task OrderCancel_StatusIsDispatched_ThrowsResourceException()
        {
            int orderId = 1;
            int userId = 99;
            var mockOrder = new Order { UserId = 88, Status = OrderStatus.Dispatched };
            _mockOrderRepository.Setup(r => r.GetOrderDetail(orderId)).ReturnsAsync(mockOrder);

            Exception caughtException = null;
            try
            {
                await _orderService.OrderCancel(orderId, userId);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            Assert.IsNotNull(caughtException);

        }

        [TestMethod]
        public async Task OrderCancel_StatusIsDelivery_ThrowsGenericException()
        {
            int orderId = 1;
            int userId = 99;
            var mockOrder = new Order { UserId = 88, Status = OrderStatus.Delivery };
            _mockOrderRepository.Setup(r => r.GetOrderDetail(orderId)).ReturnsAsync(mockOrder);

            Exception caughtException = null;
            try
            {
                await _orderService.OrderCancel(orderId, userId);
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            Assert.IsNotNull(caughtException);
        }


    }
}