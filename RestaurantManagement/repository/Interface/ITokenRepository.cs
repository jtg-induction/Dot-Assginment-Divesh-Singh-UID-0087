using RestaurantManagement.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Repository.Interface
{
    public interface ITokenRepository
    {
        Task<RefreshToken> GetTokenAsync(string token);
        Task AddTokenAsync(RefreshToken token);

        Task RevokedTokenAsync(int id);
        Task<bool> IsRevokedAsync(int id);
        Task UpdateTokenAsync(int id, string token);

    }
}
