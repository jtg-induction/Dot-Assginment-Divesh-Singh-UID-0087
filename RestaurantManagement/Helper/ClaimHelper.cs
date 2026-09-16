using RestaurantManagement.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantManagement.Helper
{
    public class ClaimHelper
    {
        public static async Task<int> GetUserIdFromClaim(System.Security.Principal.IIdentity User)
        {
            var claimsIdentity = User as ClaimsIdentity;
            var userIdClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                throw new UnauthorizedAccessException();
            }
            return currentUserId;

        }
    }
}