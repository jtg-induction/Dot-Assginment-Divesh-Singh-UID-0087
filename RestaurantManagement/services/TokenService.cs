using RestaurantManagement.Common;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace RestaurantManagement.Services
{
    public class TokenService:ITokenService
    {
      
        private readonly ITokenRepository _tokenrepository;
        public TokenService( ITokenRepository tokenrepo)
        {
            
            _tokenrepository = tokenrepo;
        }

        public string TokenGenrator()
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
    
        public string AddRefreshToken(int Id)
        {
            var refreshtoken = TokenGenrator();
            var token = new Models.Entity.RefreshToken()
            {
                UserId = Id,
                Token = refreshtoken,
            };
            _tokenrepository.Addtoken(token);

            return refreshtoken;
        }
       public  string Revoked(string token)
        {
            var refreshtoken= _tokenrepository.GetToken(token);
            if (refreshtoken != null)
            {
                _tokenrepository.RevokedToken(refreshtoken.TokenId);
                return ValidationMessages.succes;
            }
            else
            {
                return ValidationMessages.InValidToken;
            }
        }
       public  string RefreshTheToken(String token)
        {
           var refreshToken = _tokenrepository.GetToken(token);
            if (refreshToken!=null && !_tokenrepository.IsRevoked(refreshToken.TokenId))
            {
                var Newrefreshtoken = TokenGenrator();
                _tokenrepository.UpdateToken(refreshToken.TokenId, Newrefreshtoken);
                return Newrefreshtoken;
            }
            else
            {
                return ValidationMessages.Revoked;
            }

        }

        public RefreshToken Gettokendetail(string token)
        {
            return _tokenrepository.GetToken(token);
        }
    }
}