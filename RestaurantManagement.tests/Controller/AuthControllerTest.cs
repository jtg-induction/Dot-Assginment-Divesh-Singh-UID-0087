using System;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestaurantManagement.Controllers;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity; 
using RestaurantManagement.Models.Response;
using RestaurantManagement.services;

namespace RestaurantManagement.tests.Controller
{
    /// <summary>
    /// Contains unit tests for user authentication and registration operations.
    /// </summary>
    [TestClass]
    public class AuthControllerTest
    {
        /// <summary>
        /// Mock user service used by the controller under test.
        /// </summary>
        private Mock<IUserService> _mockser;

        /// <summary>
        /// Controller instance being tested.
        /// </summary>
        private AuthController _signup;

        /// <summary>
        /// Creates the mocked service and controller before each test.
        /// </summary>
        [TestInitialize]
        public void setup()
        {
            _mockser = new Mock<IUserService>();
            _signup = new AuthController(_mockser.Object);
        }

        /// <summary>
        /// Verifies that a user is successfully created when all submitted details are valid.
        /// </summary>
        [TestMethod]
        public async Task all_correct_detail()
        {
            // ARRANGE
            var incominguser = new AddUserRequest()
            {
                Name = "DIVESH",
                Email = "divesh@gmail.com",
                Password = "123234@aA",
                PhoneNumber = "1232334299",
                BirthDate = DateTime.Parse("2000-01-01 00:00:00")
            };

            // Set up a mock database user entity matching your controller property mapping expectations
            var mockCreatedUser = new User
            {
                UserId = 1,
                Name = incominguser.Name,
                Email = incominguser.Email,
                PhoneNumber = incominguser.PhoneNumber,
                BirthDate = incominguser.BirthDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Setup the async service method to return our fake created user entity object
            _mockser.Setup(r => r.AdduserAsync(It.IsAny<AddUserRequest>()))
                    .ReturnsAsync(mockCreatedUser);

            // ACT
            var response = await _signup.Signup(incominguser);

            // ASSERT
            // Cast perfectly matches the CreatedAtRoute response type from your controller
            var createdResult = response as CreatedAtRouteNegotiatedContentResult<BaseResponse<CreatedUserResponse>>;

            Assert.IsNotNull(createdResult);
        
        }
    }
}
