using RestaurantManagement.Constants;
using RestaurantManagement.Exceptions;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantManagement.Services
{
    public class TokenService:ITokenService
    {
      
        private readonly ITokenRepository _tokenRepository;
        public TokenService( ITokenRepository tokenrepo)
        {
            
            _tokenRepository = tokenrepo;
        }

        public string TokenGenerator()
        {
            var randomNumber = new byte[32];

            // RNGCryptoServiceProvider is the standard secure generator in .NET 4.8
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(randomNumber);

                // Convert to Base64 to make it string-safe for JSON/HTTP headers
                return Convert.ToBase64String(randomNumber);
            }
        }
        // Set the refresh token directly inside an HttpOnly cookie
        public void SetRefreshTokenCookie(string token)
        {
            HttpCookie cookie = new HttpCookie("X-Refresh-Token")
            {
                Value = token,
                HttpOnly = true,                             // Blocks XSS access
                Secure = true,                               // Enforces HTTPS in production
                SameSite = SameSiteMode.Strict,               // Blocks CSRF
                Expires = DateTime.UtcNow.AddDays(7)
            };
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        // Read the cookie securely from incoming requests
        public string GetRefreshTokenFromCookie()
        {
            return HttpContext.Current.Request.Cookies["X-Refresh-Token"]?.Value;
        }

        // Evict cookie upon user signout
        public void ClearRefreshTokenCookie()
        {
            if (HttpContext.Current.Response.Cookies["X-Refresh-Token"] != null)
            {
                HttpContext.Current.Response.Cookies["X-Refresh-Token"].Expires = DateTime.UtcNow.AddDays(-1);
            }
        }
    
        public async Task<string> AddRefreshTokenAsync(int id)
        {
            var refreshtoken = TokenGenerator();
            var token = new Models.Entity.RefreshToken()
            {
                UserId = id,
                Token = refreshtoken,
            };
            await _tokenRepository.AddTokenAsync(token);

            return refreshtoken;
        }
      public async Task RevokedAsync(string token)
        {
          var refreshtoken = await _tokenRepository.GetTokenAsync(token);
            if (refreshtoken != null)
            {
                await _tokenRepository.RevokedTokenAsync(refreshtoken.TokenId);
               
            }
           
                throw new ResourceException(ValidationMessages.Revoked);
               
            
        }
       public async Task<string> RefreshTheTokenAsync(string token)
        {
           var refreshToken = await _tokenRepository.GetTokenAsync(token);
            if (refreshToken != null && !await _tokenRepository.IsRevokedAsync(refreshToken.TokenId))
            {
                var Newrefreshtoken = TokenGenerator();
                await _tokenRepository.UpdateTokenAsync(refreshToken.TokenId, Newrefreshtoken);
                return Newrefreshtoken;
            }
            else
            {
                throw new ResourceException(ValidationMessages.Revoked);
            }

        }

        public async Task<RefreshToken> GetTokenDetailAsync(string token)
        {
            return await _tokenRepository.GetTokenAsync(token);
        }
    }
}