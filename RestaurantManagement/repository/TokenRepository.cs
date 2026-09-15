using NMemory.Linq;
using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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
       public RefreshToken GetToken(string token)
        {
            return _db.RefreshTokens.FirstOrDefault(e => e.Token==token);
        }
        public void Addtoken(RefreshToken token)
        {
            _db.RefreshTokens.Add(token);
            _db.SaveChanges();
        }
       public  void RevokedToken(int id)
        {
           var refreshToken= _db.RefreshTokens.Find(id);
            refreshToken.IsRevoked = true;
            _db.SaveChanges();

        }
      public   bool IsRevoked(int id)
        {
            return _db.RefreshTokens.Find(id).IsRevoked;

        }
       public  void UpdateToken(int id,string token)
        {
            var refreshtoken = _db.RefreshTokens.Find(id);
            refreshtoken.Token = token;
            refreshtoken.UpdatedAt = DateTimeOffset.UtcNow;
            _db.SaveChanges();
        }
    }
}