using RestaurantManagement.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Services.Interface
{
    public interface IObtainJwtService
    {
        string CraftJwt(User user);
    }
}
