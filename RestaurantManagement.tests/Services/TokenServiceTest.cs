using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestaurantManagement.Constants;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services;

namespace RestaurantManagement.Tests.Services
{
    [TestClass]
    public class TokenServiceTest
    {
        private Mock<ITokenRepository> _tokenRepositoryMock;
        private TokenService _tokenService;

        [TestInitialize]
        public void Setup()
        {
            _tokenRepositoryMock = new Mock<ITokenRepository>();
            _tokenService = new TokenService(_tokenRepositoryMock.Object);
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
            var stubToken = new RefreshToken { TokenId = 12, Token = oldTokenString };

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

            // ACT
            var result = await _tokenService.GetTokenDetailAsync(tokenKey);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(88, result.TokenId);
            Assert.AreEqual(55, result.UserId);
        }

       
    }
}
