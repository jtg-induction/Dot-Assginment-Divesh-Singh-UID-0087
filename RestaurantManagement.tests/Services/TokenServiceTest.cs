using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestaurantManagement.Common;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace RestaurantManagement.Tests.Services
{
    [TestClass]
    public class TokenServiceTests
    {
        private Mock<ITokenRepository> _mockRepo;
        private TokenService _service;

        [TestInitialize]
        public void Setup()
        {
            _mockRepo = new Mock<ITokenRepository>();
            _service = new TokenService(_mockRepo.Object);

            // Create authentic request and response containers using safe dummy layout properties
            var request = new HttpRequest(
                filename: string.Empty,
                url: "http://localhost/",
                queryString: string.Empty
            );
            var response = new HttpResponse(new System.IO.StringWriter());

            // Bind them together to form a perfectly isolated testing pipeline
            HttpContext.Current = new HttpContext(request, response);
        }

        [TestCleanup]
        public void Teardown()
        {
            // Reset the static context out of memory to isolate the next test run
            HttpContext.Current = null;
        }

        /// <summary>Verifies that TokenGenerator successfully creates a valid 32-byte Base64 token.</summary>
        [TestMethod]
        public void TokenGenerator_ReturnsValidBase64String()
        {
            // ACT
            string token = _service.TokenGenrator();

            // ASSERT
            Assert.IsFalse(string.IsNullOrWhiteSpace(token));

            // A 32-byte array converted to Base64 string will always be 44 characters long
            Assert.AreEqual(44, token.Length);
        }

        /// <summary>Verifies that adding a refresh token saves it to the database repository layer.</summary>
        [TestMethod]
        public void AddRefreshToken_ValidUserId_SavesTokenAndReturnsString()
        {
            // ARRANGE
            int targetUserId = 5;

            // ACT
            string resultToken = _service.AddRefreshToken(targetUserId);

            // ASSERT
            Assert.IsFalse(string.IsNullOrWhiteSpace(resultToken));
            _mockRepo.Verify(r => r.Addtoken(It.Is<RefreshToken>(t => t.UserId == targetUserId && t.Token == resultToken)), Times.Once);
        }

        /// <summary>Verifies that an existing token is marked as revoked and returns success.</summary>
        [TestMethod]
        public void Revoked_ExistingToken_UpdatesDatabaseAndReturnsSuccess()
        {
            // ARRANGE
            string tokenStr = "valid-token-to-revoke";
            var existingToken = new RefreshToken { TokenId = 12, Token = tokenStr };

            _mockRepo.Setup(r => r.GetToken(tokenStr)).Returns(existingToken);

            // ACT
            string result = _service.Revoked(tokenStr);

            // ASSERT
            Assert.AreEqual(ValidationMessages.succes, result);
            _mockRepo.Verify(r => r.RevokedToken(12), Times.Once);
        }

        /// <summary>Verifies that attempting to revoke a missing token returns an invalid token message.</summary>
        [TestMethod]
        public void Revoked_NonExistentToken_ReturnsInvalidTokenMessage()
        {
            // ARRANGE
            _mockRepo.Setup(r => r.GetToken(It.IsAny<string>())).Returns((RefreshToken)null);

            // ACT
            string result = _service.Revoked("missing-token");

            // ASSERT
            Assert.AreEqual(ValidationMessages.InValidToken, result);
            _mockRepo.Verify(r => r.RevokedToken(It.IsAny<int>()), Times.Never);
        }

        /// <summary>Verifies that refreshing an active, valid token saves a replacement code and returns it.</summary>
        [TestMethod]
        public void RefreshTheToken_ValidActiveToken_UpdatesWithNewValue()
        {
            // ARRANGE
            string oldToken = "active-refresh-token";
            var tokenEntity = new RefreshToken { TokenId = 9, Token = oldToken };

            _mockRepo.Setup(r => r.GetToken(oldToken)).Returns(tokenEntity);
            _mockRepo.Setup(r => r.IsRevoked(9)).Returns(false);

            // ACT
            string freshToken = _service.RefreshTheToken(oldToken);

            // ASSERT
            Assert.IsFalse(string.IsNullOrWhiteSpace(freshToken));
            Assert.AreNotEqual(oldToken, freshToken);
            _mockRepo.Verify(r => r.UpdateToken(9, freshToken), Times.Once);
        }

        /// <summary>Verifies that refreshing a token which is already flagged as revoked fails directly.</summary>
        [TestMethod]
        public void RefreshTheToken_AlreadyRevokedToken_ReturnsRevokedMessage()
        {
            // ARRANGE
            string badToken = "revoked-refresh-token";
            var tokenEntity = new RefreshToken { TokenId = 9, Token = badToken };

            _mockRepo.Setup(r => r.GetToken(badToken)).Returns(tokenEntity);
            _mockRepo.Setup(r => r.IsRevoked(9)).Returns(true);

            // ACT
            string result = _service.RefreshTheToken(badToken);

            // ASSERT
            Assert.AreEqual(ValidationMessages.Revoked, result);
            _mockRepo.Verify(r => r.UpdateToken(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        }

        /// <summary>Verifies that a cookie is written to the HTTP response stream with safe attributes.</summary>
        [TestMethod]
        public void SetRefreshTokenCookie_ValidString_AddsHttpOnlySecureCookie()
        {
            // ARRANGE
            string expectedToken = "secret-cookie-payload";

            // ACT
            _service.SetRefreshTokenCookie(expectedToken);

            // ASSERT
            var responseCookies = HttpContext.Current.Response.Cookies;
            Assert.IsTrue(responseCookies.AllKeys.Contains("X-Refresh-Token"));

            var targetCookie = responseCookies["X-Refresh-Token"];
            Assert.AreEqual(expectedToken, targetCookie.Value);
            Assert.IsTrue(targetCookie.HttpOnly);
            Assert.IsTrue(targetCookie.Secure);
            Assert.AreEqual(SameSiteMode.Strict, targetCookie.SameSite);
        }

        /// <summary>Verifies that cookies can be securely read from incoming requests.</summary>
        [TestMethod]
        public void GetRefreshTokenFromCookie_CookieExists_ReturnsValue()
        {
            // ARRANGE
            var cookie = new HttpCookie("X-Refresh-Token", "retrieved-value");
            HttpContext.Current.Request.Cookies.Add(cookie);

            // ACT
            string result = _service.GetRefreshTokenFromCookie();

            // ASSERT
            Assert.AreEqual("retrieved-value", result);
        }

        /// <summary>Verifies clearing a cookie shifts its expiration timestamp into the past.</summary>
        [TestMethod]
        public void ClearRefreshTokenCookie_CookieExists_ExpiresIt()
        {
            // ARRANGE
            var cookie = new HttpCookie("X-Refresh-Token")
            {
                Value = "active-session",
                Expires = DateTime.UtcNow.AddDays(7)
            };

            // Explicitly force registration into the live collection index keys
            HttpContext.Current.Response.Cookies.Set(cookie);

            // ACT
            _service.ClearRefreshTokenCookie();

            // ASSERT
            var contextualCookie = HttpContext.Current.Response.Cookies["X-Refresh-Token"];
            Assert.IsNotNull(contextualCookie, "The cookie collection returned a null reference pointer.");
            Assert.IsTrue(contextualCookie.Expires < DateTime.UtcNow, "The cookie expiration was not updated to a past date.");
        }
    }
}
