using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestaurantManagement.Constants;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.repository;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services;

namespace RestaurantManagement.Tests.Services
{
    [TestClass]
    public class TokenServiceTest
    {
        private Mock<ITokenRepository> _tokenRepositoryMock;
        private Mock<IUserRepository> _userRepository;
        private TokenService _tokenService;

        private HttpContext _httpContext;
        private StringWriter _sw;
        [TestInitialize]
        public void Setup()
        {
            _tokenRepositoryMock = new Mock<ITokenRepository>();
            _userRepository = new Mock<IUserRepository>();
            _tokenService = new TokenService(_tokenRepositoryMock.Object,_userRepository.Object);
            _httpContext = HttpContext.Current;
            _sw = new StringWriter();
            var Request = new HttpRequest("", "http://localhost/", "");
            var response = new HttpResponse(_sw);
            HttpContext.Current = new HttpContext(Request, response);
        }
        [TestMethod]
        public async Task AddRefreshTokenAsync_SavesNewTokenEntityAndReturnsString()
        {
            // ARRANGE
            int targetUserId = 99;
            _tokenRepositoryMock.Setup(r => r.AddTokenAsync(It.IsAny<RefreshToken>()))
                                .Returns(Task.CompletedTask);

            // ACT
            string generatedToken = await _tokenService.AddRefreshTokenAsync(targetUserId);

            // ASSERT
            Assert.IsNotEmpty(generatedToken);
        }
        private void SetRequestCookie(string name, string value)
        {
            var request = HttpContext.Current.Request;
            var collection = new HttpCookieCollection();
            collection.Add(new HttpCookie(name, value));
            var field = typeof(HttpRequest).GetField("_cookies",
        BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(request, collection);

        }
        [TestMethod]
        public void SetToken()
        {
            string token = "ngdjvd";
            _tokenService.SetRefreshTokenCookie(token);
            SetRequestCookie("X-Refresh-Token", token);
            var cookies = _tokenService.GetRefreshTokenFromCookie();
            Assert.AreEqual(token, cookies);
            _tokenService.ClearRefreshTokenCookie();
            var cookie = _sw.ToString();
            Assert.IsEmpty(cookie);
        }

        [TestMethod]
        public async Task RevokedAsync_ExistingToken_CallsRepositoryRevocation()
        {
            // ARRANGE
            string tokenString = "active-token-string";
            var existingToken = new RefreshToken { TokenId = 450, Token = tokenString };

            _tokenRepositoryMock.Setup(r => r.GetTokenAsync(tokenString)).ReturnsAsync(existingToken);
            _tokenRepositoryMock.Setup(r => r.RevokedTokenAsync(existingToken.TokenId)).Returns(Task.CompletedTask);

            // ACT
            await _tokenService.RevokedAsync(tokenString);

            // ASSERT
            _tokenRepositoryMock.Verify(r => r.RevokedTokenAsync(450));
        }
        [TestMethod]
        public async Task RefreshTheTokenAsync_ValidActiveToken_RotatesTokenAndReturnsNewString()
        {
            // ARRANGE
            string oldTokenString = "valid-old-token";
            var stubToken = new RefreshToken { TokenId = 12,UserId=1, Token = oldTokenString };
            _userRepository.Setup(r => r.IsActiveAsync(1)).ReturnsAsync(true);
            _tokenRepositoryMock.Setup(r => r.GetTokenAsync(oldTokenString)).ReturnsAsync(stubToken);
            _tokenRepositoryMock.Setup(r => r.IsRevokedAsync(stubToken.TokenId)).ReturnsAsync(false);
            _tokenRepositoryMock.Setup(r => r.IsExpiryed(stubToken.TokenId)).ReturnsAsync(false);
            _tokenRepositoryMock.Setup(r => r.UpdateTokenAsync(stubToken.TokenId, It.IsAny<string>()))
                                .Returns(Task.CompletedTask);

            // ACT
            string freshTokenString = await _tokenService.RefreshTheTokenAsync(oldTokenString);

            // ASSERT
            Assert.AreNotEqual(oldTokenString, freshTokenString);
        }
        [TestMethod]
        public async Task GetTokenDetailAsync_ReturnsExpectedEntityPayload()
        {
            // ARRANGE
            string tokenKey = "lookup-key";
            var expectedToken = new RefreshToken { TokenId = 88, Token = tokenKey, UserId = 55 };
            _tokenRepositoryMock.Setup(r => r.GetTokenAsync(tokenKey)).ReturnsAsync(expectedToken);
            var badToken = "hng";
            // ACT
            Exception exception = null;
            try
            {
                string result = await _tokenService.RefreshTheTokenAsync(badToken);
            }
            catch (Exception e)
            {
                exception = e;
            }

            // ASSERT
            Assert.IsNotNull(exception);
        }
        [TestMethod]
        public async Task GetTokenDetail()
        {
            // ARRANGE
            string token1 = "ksndff";
                var token = new RefreshToken { Token = "jffrufnrf" };
            _tokenRepositoryMock.Setup(r => r.GetTokenAsync(token1)).ReturnsAsync(token);

            // ACT
            var generatedToken = await _tokenService.GetTokenDetailAsync(token1);

            // ASSERT
            Assert.IsNotNull(generatedToken);
        }


    }
}
