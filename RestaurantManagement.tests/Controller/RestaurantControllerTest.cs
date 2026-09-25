using Moq;
using RestaurantManagement.Constants;
using RestaurantManagement.Controllers;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace RestaurantManagement.tests.Controller
{
    [TestClass]
    public  class RestaurantControllerTest
    {
        private Mock<IMenuService> _menuService;
        private Mock<IRestaurantService> _restaurantService;
        private RestaurantController _restaurantController;
        [TestInitialize]
        public void setup()
        {
            _menuService = new Mock<IMenuService>();
            _restaurantService = new Mock<IRestaurantService>();
            _restaurantController = new RestaurantController(_restaurantService.Object,_menuService.Object);
        }
        [TestMethod]
        public async Task activerestaurant()
        {
            _restaurantService.Setup(e => e.GetRestaurantsAsync()).ReturnsAsync(new List<ActiveRestaurantResponse> { new ActiveRestaurantResponse { RestaurantId = 1, Name = "Test" } });
            var response= await _restaurantController.GetRestaurants();
            //Assert.Fail(response.GetType().FullName);
            var okk = response as OkNegotiatedContentResult<BaseResponse<List<ActiveRestaurantResponse>>>;
            Assert.IsNotNull(okk);
        }
        [TestMethod]
        public  async Task menuitem()
        {
            _menuService.Setup(e => e.GetMenuItemsAsync(1)).ReturnsAsync(It.IsAny<List<GetMenuItemResponse>>());
            var response = await _restaurantController.GetMenuByRestaurantId(1);
            var okk = response as OkNegotiatedContentResult<BaseResponse<List<GetMenuItemResponse>>>;
            Assert.IsNotNull(okk);
        }
        [TestMethod]
        public async Task AddRestaurant_ValidRequest_ReturnsCreatedWithSuccessResponse()
        {
            // Arrange

           _restaurantService
                .Setup(s => s.AddRestaurant(It.IsAny<AddRestaurantRequest>()))
                .Returns(Task.CompletedTask); // Simulates an async void (Task) return

            // Act
            var result = await _restaurantController.AddRestaurant(It.IsAny<AddRestaurantRequest>());

            // Assert
            var createdResult = result as CreatedNegotiatedContentResult<BaseResponse<string>>;
            Assert.IsNotNull(createdResult);

           
        }
        [TestMethod]
        public async Task AddRestaurantOwner_ValidRequest_ReturnsCreatedWithSuccessResponse()
        {
            // Arrange
            _restaurantService
                .Setup(s => s.AddRestaurantowner(It.IsAny<AddRestaurantOwnerRequest>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _restaurantController.AddRestaurantOwner(It.IsAny<AddRestaurantOwnerRequest>());

            // Assert
            var createdResult = result as OkNegotiatedContentResult<BaseResponse<string>>;
            Assert.IsNotNull(createdResult);
        }
    }
}
