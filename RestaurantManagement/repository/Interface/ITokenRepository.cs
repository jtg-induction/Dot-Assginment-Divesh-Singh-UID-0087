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
        RefreshToken GetToken(string token);
        void Addtoken(RefreshToken token);

        void RevokedToken(int id);
        bool IsRevoked(int id);
        void UpdateToken(int id, string token);

    }
}
