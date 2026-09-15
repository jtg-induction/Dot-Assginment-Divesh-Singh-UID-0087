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
        string TokenGenrator();
        string GetRefreshTokenFromCookie();
        void ClearRefreshTokenCookie();
        void SetRefreshTokenCookie(string token);
        string AddRefreshToken(int Id);
        string Revoked(string token);
        string RefreshTheToken(String token);
        RefreshToken Gettokendetail(String token);
     

    }
}
