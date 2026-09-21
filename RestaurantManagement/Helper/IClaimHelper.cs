using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Helper
{
    public interface IClaimHelper
    {
        Task<int> GetUserIdFromClaim(System.Security.Principal.IIdentity User);
    }
}
