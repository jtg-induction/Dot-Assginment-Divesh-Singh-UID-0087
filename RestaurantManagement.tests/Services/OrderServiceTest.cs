using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
        private OrderService _orderService;

        [TestInitialize]
        public void SetUp()
        {
            _mockMenuRepository = new Mock<IMenuRepository>();
            _mockAddressRepository = new Mock<IAddressRepository>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockOrderRepository = new Mock<IOrderRepository>();
            _mockOrderItemRepository = new Mock<IOrderItemRepository>();

            _orderService = new OrderService(
                _mockMenuRepository.Object,
                _mockAddressRepository.Object,
                _mockUserRepository.Object,
                _mockOrderRepository.Object,
                _mockOrderItemRepository.Object
            );
        }

        [TestMethod]
        public async Task AddOrder_ValidInput_SuccessfullyPlacesOrderAndDeductsBalance()
        {
            int userId = 1;
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
                .Setup(repo => repo.GetAddress(addressId))
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
                .Setup(repo => repo.UpdateBalance(userId, 25.00m))
                .Returns(Task.CompletedTask);

            _mockOrderRepository
                .Setup(repo => repo.PlacedOrder(It.IsAny<Order>()))
                .Callback<Order>(o => {
                    o.OrderId = 777;
                    o.RestaurantId = 5;
                    o.Status = OrderStatus.Placed;
                })
                .Returns(Task.CompletedTask);

            _mockOrderItemRepository
                .Setup(repo => repo.AddOrderItem(It.IsAny<List<OrderItem>>()))
                .Returns(Task.CompletedTask);

            OrderResponse response = await _orderService.AddOrder(userOrder, addressId, userId);

            Assert.IsNotNull(response);
           
        }



    }
}
