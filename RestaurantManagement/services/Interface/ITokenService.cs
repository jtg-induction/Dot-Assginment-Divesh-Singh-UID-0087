using RestaurantManagement.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Services.Interface
{
        public interface ITokenService
        {
                string TokenGenerator();
                string GetRefreshTokenFromCookie();
                void ClearRefreshTokenCookie();
                void SetRefreshTokenCookie(string token);
                Task<string> AddRefreshTokenAsync(int id);
                Task RevokedAsync(string token);
                Task<string> RefreshTheTokenAsync(string token);
                Task<RefreshToken> GetTokenDetailAsync(string token);
        Task RevokedAllToken(int userid);


        }
}
