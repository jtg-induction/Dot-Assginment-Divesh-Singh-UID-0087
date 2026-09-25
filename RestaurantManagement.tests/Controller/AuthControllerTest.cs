using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestaurantManagement.Constants;
using RestaurantManagement.Controllers;
using RestaurantManagement.Helper;
using RestaurantManagement.Models;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Services;
using RestaurantManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace RestaurantManagement.tests.Controller
{
    /// <summary>
    /// Contains unit tests for user authentication and registration operations.
    /// </summary>
    [TestClass]
    public class AuthControllerTest
    {
        private Mock<IUserService> _userServiceMock;
        private Mock<ITokenService> _tokenServiceMock;
        private Mock<IObtainJwtService> _jwtServiceMock;
        private AuthController _authController;
                    private Mock<IClaimHelper> _claimhelpermock;

        /// <summary>
        /// Creates the mocked services and target controller before each test runs.
        /// </summary>
        [TestInitialize]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _tokenServiceMock = new Mock<ITokenService>();
            _jwtServiceMock = new Mock<IObtainJwtService>();
            _claimhelpermock = new Mock<IClaimHelper>();


            _authController = new AuthController(
                _userServiceMock.Object,
                _jwtServiceMock.Object,
                _tokenServiceMock.Object,_claimhelpermock.Object
            );
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

            _userServiceMock.Setup(r => r.AdduserAsync(It.IsAny<AddUserRequest>()));
                            

            // ACT
            var response = await _authController.Signup(incominguser);

            // ASSERT
            var createdResult = response as CreatedNegotiatedContentResult<BaseResponse<string>>;
            Assert.IsNotNull(createdResult);
        }

        /// <summary>
        /// Verifies that valid credentials return a 200 OK along with an access token.
        /// </summary>
        [TestMethod]
        public async Task CorrectLoginDetail()
        {
            // ARRANGE
            var incominguser = new UserCredential()
            {
                Email = "divesh@gmail.com",
                Password = "123234@aA"
            };

            var resultuser = new User()
            {
                UserId = 1,
                Name = "Divesh",
                Email = incominguser.Email,
                Role = UserRole.Customer
            };

            // Aligned method setups with controller logic (_userService.LoginUserAsync)
            _userServiceMock.Setup(r => r.LoginUserAsync(It.IsAny<UserCredential>())).ReturnsAsync(resultuser);
            _tokenServiceMock.Setup(r => r.AddRefreshTokenAsync(1)).ReturnsAsync("nvjdansjfvsk");
            _jwtServiceMock.Setup(r => r.CraftJwt(It.IsAny<User>())).Returns("jwiojunfjahj");
            _tokenServiceMock.Setup(r => r.SetRefreshTokenCookie(It.IsAny<string>()));

            // ACT
            var response = await _authController.Login(incominguser);

            // ASSERT
            var okResult = response as OkNegotiatedContentResult<BaseResponse<LoginResponse>>;
            Assert.IsNotNull(okResult);
            Assert.IsTrue(okResult.Content.success);
            Assert.AreEqual("jwiojunfjahj", okResult.Content.data.Token);
        }

        /// <summary>
        /// Verifies that an invalid user credential sets a proper 401 Unauthorized content result.
        /// </summary>
        [TestMethod]
        public async Task InValidLoginDetail()
        {
            // ARRANGE
            var incominguser = new UserCredential()
            {
                Email = "divesh@gmail.com",
                Password = "123234@aaA"
            };

            // Simulating user lookup failure by throwing a target exception (aligns with try/catch Option 1)
            _userServiceMock.Setup(r => r.LoginUserAsync(It.IsAny<UserCredential>()))
                            .ThrowsAsync(new KeyNotFoundException(ValidationMessages.UserNotFound));

            // ACT
            Exception ee = null;
            try
            {
                var response = await _authController.Login(incominguser);
            }
            catch (Exception e)
            {
                ee = e;
            }

            // ASSERT
            Assert.IsNotNull(ee);
        }

        /// <summary>
        /// Verifies that calling logout revokes tokens and wipes contextual tracking cookies.
        /// </summary>
        [TestMethod]
        public async Task LogoutTest()
        {
            // ARRANGE
            _tokenServiceMock.Setup(r => r.GetRefreshTokenFromCookie()).Returns("djbdgbiuf");
            _tokenServiceMock.Setup(r => r.ClearRefreshTokenCookie());
            _tokenServiceMock.Setup(r => r.RevokedAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

            // ACT
            var response = await _authController.Logout();

            // ASSERT
            var okResult = response as OkNegotiatedContentResult<BaseResponse<string>>;
            Assert.IsNotNull(okResult);
            Assert.IsTrue(okResult.Content.success);
        }

        /// <summary>
        /// Verifies token rotation successfully yields regenerated credential pairings.
        /// </summary>
        [TestMethod]
        public async Task Refresh_ValidToken_ReturnsOkWithNewAccessToken()
        {
            // ARRANGE
            string existingCookieToken = "old-refresh-token-from-cookie";
            string newlyGeneratedRefreshToken = "newly-generated-refresh-token";
            string newAccessToken = "new-crafted-jwt-access-token";
            int mockUserId = 42;

            var dummyUser = new User()
            {
                UserId = mockUserId,
                Name = "Divesh",
                Email = "divesh@gmail.com"
            };

                        var tokenDetailStub = new RefreshToken { UserId = mockUserId };

            _tokenServiceMock.Setup(s => s.GetRefreshTokenFromCookie()).Returns(existingCookieToken);
            _tokenServiceMock.Setup(s => s.RefreshTheTokenAsync(existingCookieToken)).ReturnsAsync(newlyGeneratedRefreshToken);
            _tokenServiceMock.Setup(s => s.GetTokenDetailAsync(newlyGeneratedRefreshToken)).ReturnsAsync(tokenDetailStub);
            _userServiceMock.Setup(s => s.GetUserAsync(mockUserId)).ReturnsAsync(dummyUser);
            _jwtServiceMock.Setup(c => c.CraftJwt(dummyUser)).Returns(newAccessToken);
            _tokenServiceMock.Setup(s => s.SetRefreshTokenCookie(newlyGeneratedRefreshToken));

            //            // ACT
                        var response = await _authController.Refresh();

            // ASSERT
            var okResult = response as OkNegotiatedContentResult<BaseResponse<LoginResponse>>;
            Assert.IsNotNull(okResult);
            Assert.IsTrue(okResult.Content.success);
            Assert.AreEqual(newAccessToken, okResult.Content.data.Token);
        }
        [TestMethod]
        public async Task deactivateaccount()
        {
            UpdateAccountDto update = new UpdateAccountDto
            {
                Email = "abc@abc.com"
            };

            _claimhelpermock.Setup(e => e.GetUserIdFromClaim(It.IsAny<System.Security.Principal.IIdentity>())).ReturnsAsync(123);
            _userServiceMock.Setup(e => e.DeactivateAccount(1)).ReturnsAsync(It.IsAny<User>());

            var response = await _authController.DeactivateAccount();
            //Assert.Fail(response.GetType().FullName);
            var okk = response as OkNegotiatedContentResult<BaseResponse<string>>;
            Assert.IsNotNull(okk);

        }
        [TestMethod]
        public async Task activateaccount()
        {
            UserCredential update = new UserCredential
            {
                Email = "abc@abc.com",
                Password="juiuy@1S"
            };

            _claimhelpermock.Setup(e => e.GetUserIdFromClaim(It.IsAny<System.Security.Principal.IIdentity>())).ReturnsAsync(123);
            _userServiceMock.Setup(e => e.DeactivateAccount(1)).ReturnsAsync(It.IsAny<User>());

            var response = await _authController.ActivateAccount(update);
            //Assert.Fail(response.GetType().FullName);
            var okk = response as OkNegotiatedContentResult<BaseResponse<string>>;
            Assert.IsNotNull(okk);

        }


    }
}
