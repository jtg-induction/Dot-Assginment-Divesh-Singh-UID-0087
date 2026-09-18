using System;
using System.Data.Common;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Effort;
using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Repository;

namespace RestaurantManagement.tests.Repository
{
    [TestClass]
    public class TokenRepositoryTest
    {
        private ApplicationDbContext _context;
        private TokenRepository _tokenRepo;

        [TestInitialize]
        public void Setup()
        {
            DbConnection connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new ApplicationDbContext(connection);
            _tokenRepo = new TokenRepository(_context);
        }

        /// <summary>Helper method to ensure a valid User exists in the transient database before token actions.</summary>
        private async Task SeedUserAsync(int userId)
        {
            var user = new User
            {
                UserId = userId,
                Name = $"Test User {userId}",
                Password = "SecurePassword123!",
                Email = $"user{userId}@example.com",
                BirthDate = new DateTime(1990, 1, 1),
                PhoneNumber = $"555-000-{userId:D4}", // Generates unique phone numbers like 555-000-0002
                Balance = 1000,
                Role = RestaurantManagement.Models.Enum.UserRole.Customer // Use a valid enum value from your project
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        [TestMethod]
        public async Task GetTokenAsync_ExistingToken_ReturnsRefreshToken()
        {
            // ARRANGE
            await SeedUserAsync(1); // Seed User 1

            var tokenEntity = new RefreshToken
            {
                Token = "valid-refresh-token-string",
                UserId = 1,
                IsRevoked = false,
                UpdatedAt = DateTime.UtcNow
            };
            _context.RefreshTokens.Add(tokenEntity);
            await _context.SaveChangesAsync();

            // ACT
            var result = await _tokenRepo.GetTokenAsync("valid-refresh-token-string");

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual("valid-refresh-token-string", result.Token);
            Assert.AreEqual(1, result.UserId);
        }

        [TestMethod]
        public async Task AddTokenAsync_SavesTokenCorrectly()
        {
            // ARRANGE
            var user = new User
            {
                Name = "Test User Two",
                Password = "SecurePassword123!",
                Email = "user2@example.com",
                BirthDate = new DateTime(1990, 1, 1),
                PhoneNumber = "555-000-0002",
                Balance = 1000,
                Role = RestaurantManagement.Models.Enum.UserRole.Customer
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(); 

            // 2. Map the generated UserId straight to the token
            var newToken = new RefreshToken
            {
                Token = "brand-new-token",
                UserId = user.UserId, 
                IsRevoked = false,
                UpdatedAt = DateTime.UtcNow
            };

            // ACT
            await _tokenRepo.AddTokenAsync(newToken);

            // ASSERT
            var savedToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == "brand-new-token");
            Assert.IsNotNull(savedToken);
            Assert.AreEqual(user.UserId, savedToken.UserId);
        }

        [TestMethod]
        public async Task RevokedTokenAsync_SetsIsRevokedToTrue()
        {
            // ARRANGE
            await SeedUserAsync(1); // Seed User 1

            var tokenEntity = new RefreshToken()
            {
                Token = "token-to-revoke",
                UserId = 1,
                IsRevoked = false,
                UpdatedAt = DateTime.UtcNow
            };
            _context.RefreshTokens.Add(tokenEntity);
            await _context.SaveChangesAsync();

            // ACT
            await _tokenRepo.RevokedTokenAsync(tokenEntity.TokenId);

            // ASSERT
            var updatedToken = await _context.RefreshTokens.FindAsync(tokenEntity.TokenId);
            Assert.IsNotNull(updatedToken);
            Assert.IsTrue(updatedToken.IsRevoked);
        }

        [TestMethod]
        public async Task IsRevokedAsync_EvaluatesStateAccurately()
        {
            // ARRANGE
            await SeedUserAsync(1); // Seed User 1

            var clearToken = new RefreshToken { Token = "clear", UserId = 1, IsRevoked = false, UpdatedAt = DateTime.UtcNow };
            var badToken = new RefreshToken { Token = "bad", UserId = 1, IsRevoked = true, UpdatedAt = DateTime.UtcNow };

            _context.RefreshTokens.Add(clearToken);
            _context.RefreshTokens.Add(badToken);
            await _context.SaveChangesAsync();

            // ACT
            bool resultClear = await _tokenRepo.IsRevokedAsync(clearToken.TokenId);
            bool resultBad = await _tokenRepo.IsRevokedAsync(badToken.TokenId);

            // ASSERT
            Assert.IsFalse(resultClear);
            Assert.IsTrue(resultBad);
        }

        [TestMethod]
        public async Task UpdateTokenAsync_AltersTokenStringAndTimestamp()
        {
            // 1. Arrange: Seed a valid user and get their real ID
            var testUser = new User
            {
                Name = "Token Update User",
                Password = "SecurePassword123!",
                Email = "updateuser@example.com",
                BirthDate = new DateTime(1990, 1, 1),
                PhoneNumber = "555-000-9999",
                Balance = 1000,
                Role = RestaurantManagement.Models.Enum.UserRole.Customer
            };
            _context.Users.Add(testUser);
            await _context.SaveChangesAsync(); 

            
            var originalTime = DateTime.UtcNow.AddHours(-1);
            var testToken = new RefreshToken
            {
                Token = "old-token-string",
                UserId = testUser.UserId, 
                IsRevoked = false,
                UpdatedAt = originalTime
            };
            _context.RefreshTokens.Add(testToken);
            await _context.SaveChangesAsync();

            // 3. Act: Invoke your repository method to update the token
            string newTokenString = "brand-new-token-string";
            await _tokenRepo.UpdateTokenAsync(testToken.TokenId, newTokenString);

            // 4. Assert: Pull a fresh copy from context to verify changes
            var updatedToken = await _context.RefreshTokens.FindAsync(testToken.TokenId);

            Assert.IsNotNull(updatedToken);
            Assert.AreEqual(newTokenString, updatedToken.Token);
            Assert.IsTrue(updatedToken.UpdatedAt > originalTime);
        }

        [TestMethod]
        public async Task IsExpiryed_EvaluatesLifespanBoundaries()
        {
            // ARRANGE
            await SeedUserAsync(1); // Seed User 1

            var freshToken = new RefreshToken
            {
                Token = "fresh",
                UserId = 1,
                UpdatedAt = DateTime.UtcNow.AddDays(-2)
            };

            var staleToken = new RefreshToken
            {
                Token = "stale",
                UserId = 1,
                UpdatedAt = DateTime.UtcNow.AddDays(-8)
            };

            _context.RefreshTokens.Add(freshToken);
            _context.RefreshTokens.Add(staleToken);
            await _context.SaveChangesAsync();

            // ACT
            bool isFreshExpired = await _tokenRepo.IsExpiryed(freshToken.TokenId);
            bool isStaleExpired = await _tokenRepo.IsExpiryed(staleToken.TokenId);

            // ASSERT
            Assert.IsFalse(isFreshExpired);
            Assert.IsTrue(isStaleExpired);
        }
    }
}
