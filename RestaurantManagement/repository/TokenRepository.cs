using NMemory.Linq;
using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantManagement.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used to access user records.</param>
        public TokenRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public async Task<RefreshToken> GetTokenAsync(string token)
        {
            return await _db.RefreshTokens.FirstOrDefaultAsync(e => e.Token == token);
        }
        public async Task AddTokenAsync(RefreshToken token)
        {
            _db.RefreshTokens.Add(token);
            await _db.SaveChangesAsync();
        }
        public async Task RevokedTokenAsync(int id)
        {
            var refreshToken = await _db.RefreshTokens.FindAsync(id);
            refreshToken.IsRevoked = true;
            await _db.SaveChangesAsync();

        }
        public async Task<bool> IsRevokedAsync(int id)
        {
            var refreshToken = await _db.RefreshTokens.FindAsync(id);
            return refreshToken.IsRevoked;

        }
        public async Task UpdateTokenAsync(int id, string token)
        {
            var refreshtoken = await _db.RefreshTokens.FindAsync(id);
            refreshtoken.Token = token;
            refreshtoken.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
        public async Task<bool> IsExpiryed(int id)
        {
            var refreshtoken = await _db.RefreshTokens.FindAsync(id);

            return (DateTime.UtcNow - refreshtoken.UpdatedAt).TotalSeconds > 604800;
        }
    }
}