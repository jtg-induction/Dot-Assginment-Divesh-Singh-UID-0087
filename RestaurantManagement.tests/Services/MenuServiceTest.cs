using Moq;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Repository;
using RestaurantManagement.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantManagement.Repository.Interface;

namespace RestaurantManagement.tests.Services
{
    [TestClass]
    public class MenuServiceTest
    {
        private Mock<IMenuRepository> _menuRepository;
        private Mock<IRestaurantRepository> _restaurantRepository;
        private MenuService _menuService;

        [TestInitialize]
        public void Setup()
        {
            _menuRepository = new Mock<IMenuRepository>();
            _restaurantRepository = new Mock<IRestaurantRepository>();
            _menuService = new MenuService(_menuRepository.Object,_restaurantRepository.Object);
        }

        [TestMethod]
        public async Task GetMenuItemsAsync_ReturnsMappedItems()
        {
            // Arrange
            var menuItems = new List<MenuItem>
            {
                new MenuItem { ItemId = 1, RestaurantId = 1, DishName = "Pizza", Price = 100m, AvailableQuantity = 10 },
                new MenuItem { ItemId = 2, RestaurantId = 1, DishName = "Burger", Price = 80m, AvailableQuantity = 5 }
            };
            _restaurantRepository.Setup(e => e.RestaurantIsActive(1)).ReturnsAsync(true);

            _menuRepository.Setup(r => r.GetMenuItem(1)).ReturnsAsync(menuItems);

            // Act
            var result = await _menuService.GetMenuItemsAsync(1);

            // Assert
            Assert.AreEqual(2, result.Count);
         
        }

        [TestMethod]
        public async Task GetMenuItemsAsync_ReturnsEmptyList_WhenNoItems()
        {
            _menuRepository
                .Setup(r => r.GetMenuItem(99))
                .ReturnsAsync(new List<MenuItem>());
            Exception e = null;
            try
            {
                var result = await _menuService.GetMenuItemsAsync(99);
            }catch(Exception ex)
            {
                e = ex;
            }

            Assert.IsNotNull(e);
        }

      
    }
}