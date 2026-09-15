using Effort;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Repository;
using System;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.tests.Repository
{
    [TestClass]
    /// <summary>
    /// Contains integration tests for <see cref="TokenRepository"/> using a transient database.
    /// </summary>
    public class TokenRepositoryTest
    {
        private ApplicationDbContext _context;
        private TokenRepository _tokenrepo;
        private int _defaultUserId;

        [TestInitialize]
        public void Setup()
        {
            DbConnection connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new ApplicationDbContext(connection);
            _tokenrepo = new TokenRepository(_context);

            // Seed a common default user to fulfill foreign key constraints automatically across tests
            var seedUser = new User
            {
                Email = "defaultowner@gmail.com",
                Name = "Default Token Owner",
                Password = "securepassword",
                PhoneNumber = "987654321",
                BirthDate = DateTime.Parse("2000-01-01"),
                Balance = 0,
                Role = UserRole.Customer
            };
            _context.Users.Add(seedUser);
            _context.SaveChanges();

            // Capture the generated identity primary key
            _defaultUserId = seedUser.UserId;
        }

        /// <summary>Verifies that an existing token string can be retrieved.</summary>
        [TestMethod]
        public async Task CheckTokenIsRetrieved()
        {
            var testToken = new RefreshToken
            {
                Token = "token-abc-123",
                IsRevoked = false,
                UpdatedAt = DateTime.UtcNow,
                UserId = _defaultUserId
            };

            _context.RefreshTokens.Add(testToken);
            _context.SaveChanges();

            var result = await _tokenrepo.GetTokenAsync("token-abc-123");

            Assert.IsNotNull(result);
            Assert.AreEqual("token-abc-123", result.Token);
        }

        /// <summary>Verifies that a new refresh token can be added to the database.</summary>
        [TestMethod]
        public async Task AddToken()
        {
            // Optional: Arrange a custom distinct user if needed for this specific test
            var linkedUser = new User
            {
                Email = "tokenowner@gmail.com",
                Name = "Token Owner",
                Password = "securepassword",
                PhoneNumber = "12345678",
                BirthDate = DateTime.Parse("2000-01-01"),
                Balance = 0,
                Role = UserRole.Customer
            };
            _context.Users.Add(linkedUser);
            _context.SaveChanges();

            var testToken = new RefreshToken
            {
                Token = "token-new-456",
                IsRevoked = false,
                UpdatedAt = DateTime.UtcNow,
                UserId = linkedUser.UserId
            };

            // ACT
            await _tokenrepo.AddTokenAsync(testToken);

            // ASSERT
            var result = _context.RefreshTokens.FirstOrDefault(t => t.Token == "token-new-456");
            Assert.IsNotNull(result);
            Assert.AreEqual(testToken.Token, result.Token);
            Assert.AreEqual(linkedUser.UserId, result.UserId);
        }

        /// <summary>Verifies that an existing token can be revoked successfully.</summary>
        [TestMethod]
        public async Task RevokedToken()
        {
            var testToken = new RefreshToken
            {
                Token = "token-to-revoke",
                IsRevoked = false,
                UpdatedAt =DateTime.UtcNow,
                UserId = _defaultUserId
            };

            _context.RefreshTokens.Add(testToken);
            _context.SaveChanges();

            // ACT
            await _tokenrepo.RevokedTokenAsync(testToken.TokenId);

            // ASSERT
            var result = _context.RefreshTokens.Find(testToken.TokenId);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsRevoked);
        }

        /// <summary>Verifies that the revocation status check reports the correct value.</summary>
        [TestMethod]
        public async Task CheckIsRevokedStatus()
        {
            var activeToken = new RefreshToken { Token = "active-token", IsRevoked = false, UpdatedAt = DateTime.UtcNow, UserId = _defaultUserId };
            var revokedToken = new RefreshToken { Token = "revoked-token", IsRevoked = true, UpdatedAt = DateTime.UtcNow, UserId = _defaultUserId };

            _context.RefreshTokens.Add(activeToken);
            _context.RefreshTokens.Add(revokedToken);
            _context.SaveChanges();

            // ACT & ASSERT
            Assert.IsFalse(await _tokenrepo.IsRevokedAsync(activeToken.TokenId));
            Assert.IsTrue(await _tokenrepo.IsRevokedAsync(revokedToken.TokenId));
        }

        /// <summary>Verifies that a token string and its modification timestamp can be updated.</summary>
        [TestMethod]
        public async Task UpdateToken()
        {
            var testToken = new RefreshToken
            {
                Token = "old-token-string",
                IsRevoked = false,
                UpdatedAt = DateTime.UtcNow.AddDays(-5),
                UserId = _defaultUserId
            };

            _context.RefreshTokens.Add(testToken);
            _context.SaveChanges();

            string updatedString = "new-token-string-789";

            // ACT
            await _tokenrepo.UpdateTokenAsync(testToken.TokenId, updatedString);

            // ASSERT
            var result = _context.RefreshTokens.Find(testToken.TokenId);
            Assert.IsNotNull(result);
            Assert.AreEqual(updatedString, result.Token);

            // Verifies the modification timestamp was updated to roughly now (within 5 seconds)
            Assert.IsTrue((DateTime.UtcNow - result.UpdatedAt).TotalSeconds < 5);
        }
    }
}
