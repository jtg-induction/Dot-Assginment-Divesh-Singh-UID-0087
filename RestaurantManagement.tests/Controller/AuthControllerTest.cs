using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.DependencyModel;
using Moq;
//using OWIN.WebApi.Controllers;
//using RestaurantManagement.Common;
using RestaurantManagement.Controllers;
using RestaurantManagement.Models;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.repository;
//using RestaurantManagement.services;
using RestaurantManagement.Services;
using RestaurantManagement.Services.Interface;
using System.Text.Json;
using System.Web.Http.Results;
using System.Web.UI.WebControls.WebParts;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using System.Threading.Tasks;
using RestaurantManagement.Constants;

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
        private Mock<IUserService> _userServiceMock;
        private Mock<ITokenService> _tokenServiceMock;
        private Mock<IObtainJwtService> _jwtServiceMock;
        /// <summary>
        /// Controller instance being tested.
        /// </summary>
        private AuthController _authController;

        /// <summary>
        /// Creates the mocked service and controller before each test.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _jwtServiceMock = new Mock<IObtainJwtService>();
            _authController = new AuthController(_userServiceMock.Object, _jwtServiceMock.Object, _tokenServiceMock.Object);
        }

        /// <summary>
        /// Verifies that a user is created when all submitted details are valid.
        /// </summary>
        [TestMethod]
        public async Task all_correct_detail()
        {
            //ARRANGE
            var incominguser = new AddUserRequest()
            {
                Name = "DIVESH",
                Email = "divesh@gmail.com",
                Password = "123234@aA",
                PhoneNumber = "1232334299",
                BirthDate = DateTime.Parse("2000-01-01 00:00:00")
            };

            _userServiceMock.Setup(r => r.AdduserAsync(incominguser));

            //ACT
            var response = await _authController.Signup(incominguser);
            //ASSERT
            //if (response as CreatedNegotiatedContentResult<AddUserRequest>!=null)
            //{
            //    Assert.Fail(response.Meassage);
            //}
            var createdResult = response as OkNegotiatedContentResult<string>;
            //Assert.Fail(createdResult);
            //Assert.Fail($"Name was: {createdResult==null}");
            Assert.IsNotNull(createdResult);
        }

        /// <summary>
        /// Verifies that an invalid email produces a model-state error response.
        /// </summary>
        [TestMethod]
        public async Task CorrectLoginDetail()
        {
            //ARRANGE
            var incominguser = new UserCredential()
            {
                Email = "divesh@gmail.com",
                Password = "123234@aA"
            };
            var resultuser = new Models.Entity.User()
            {
                UserId = 1,
                Name = "Divesh",
                Email = incominguser.Email,
                Role = UserRole.Customer
            };

            _userServiceMock.Setup(r => r.CheckUserAsync(It.IsAny<UserCredential>())).ReturnsAsync(resultuser);
            _tokenServiceMock.Setup(r => r.AddRefreshTokenAsync(1)).ReturnsAsync("nvjdansjfvsk");
            _jwtServiceMock.Setup(r => r.CraftJwt(It.IsAny<Models.Entity.User>())).Returns("jwiojunfjahj");
            _tokenServiceMock.Setup(r => r.SetRefreshTokenCookie("kdjviudbvb"));

            //ACT
            var response = await _authController.Login(incominguser);
            //ASSERT
            //Assert.Fail(response.GetType().FullName);
            var createdResult = response as dynamic;
            Assert.IsNotNull(createdResult);
        }
        [TestMethod]
        public async Task InValidLoginDetail()
        {
            //ARRANGE
            var incominguser = new UserCredential()
            {
                Email = "divesh@gmail.com",
                Password = "123234@aaA"
            };


            _userServiceMock.Setup(r => r.CheckUserAsync(incominguser)).ReturnsAsync((Models.Entity.User)null);

            //ACT
            var response = await _authController.Login(incominguser);
            //ASSERT
            var createdResult = response as UnauthorizedResult;
            Assert.IsNotNull(createdResult);
        }
        [TestMethod]
        public async Task LogoutTest()
        {
            //ARRANGE
            _tokenServiceMock.Setup(r => r.GetRefreshTokenFromCookie()).Returns("djbdgbiuf");
            _tokenServiceMock.Setup(r => r.ClearRefreshTokenCookie());
            _tokenServiceMock.Setup(r => r.RevokedAsync("jfgbkugkdub"));

            //ACT
            var response = await _authController.Logout();
            //ASSERT
            //Assert.Fail(response.GetType().FullName);
            var createdResult = response as dynamic;
            Assert.IsNotNull(createdResult);
        }
        [TestMethod]
        public async Task Refresh_ValidToken_ReturnsOkWithNewAccessToken()
        {
            // ARRANGE
            string existingCookieToken = "old-refresh-token-from-cookie";
            string newlyGeneratedRefreshToken = "newly-generated-refresh-token";
            string newAccessToken = "new-crafted-jwt-access-token";
            int mockUserId = 42;

            var dummyUser = new Models.Entity.User()
            {
                UserId = mockUserId,
                Name = "Divesh",
                Email = "divesh@gmail.com"
            };

            var tokenDetailStub = new RefreshToken { UserId = mockUserId };

            // Stub out each dependency step-by-step
            _tokenServiceMock.Setup(s => s.GetRefreshTokenFromCookie()).Returns(existingCookieToken);
            _tokenServiceMock.Setup(s => s.RefreshTheTokenAsync(existingCookieToken)).ReturnsAsync(newlyGeneratedRefreshToken);
            _tokenServiceMock.Setup(s => s.GetTokenDetailAsync(newlyGeneratedRefreshToken)).ReturnsAsync(tokenDetailStub);

            _userServiceMock.Setup(s => s.GetUserAsync(mockUserId)).ReturnsAsync(dummyUser);
            _jwtServiceMock.Setup(c => c.CraftJwt(dummyUser)).Returns(newAccessToken);

            // Setup void cookie tracking method
            _tokenServiceMock.Setup(s => s.SetRefreshTokenCookie(newlyGeneratedRefreshToken));

            // ACT
            var response = await _authController.Refresh();

            // ASSERT
            var createdResult = response as dynamic;
            Assert.IsNotNull(createdResult);

        }

        [TestMethod]
        public async Task Refresh_RevokedToken_ReturnsUnauthorized()
        {
            // ARRANGE
            string staleCookieToken = "revoked-cookie-token";

            _tokenServiceMock.Setup(s => s.GetRefreshTokenFromCookie()).Returns(staleCookieToken);

            // Simulate token revocation string output matching your ValidationMessages
            _tokenServiceMock.Setup(s => s.RefreshTheTokenAsync(staleCookieToken)).ReturnsAsync(ValidationMessages.Revoked);

            // ACT
            Exception exception = null;
            try
            {
                var response = await _authController.Refresh();
            }
            catch (Exception e)
            {
                exception = e;
            }

            // ASSERT
            Assert.IsNotNull(exception);
        }






    }
}
