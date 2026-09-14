using RestaurantManagement.Models.Dto;
using System.Threading.Tasks;

namespace RestaurantManagement.services
{
        /// <summary>
        /// Defines operations for managing users.
        /// </summary>
        public interface IUserService
        {
                /// <summary>
                /// Adds a new user.
                /// </summary>
                /// <param name="adduser">The user details to add.</param>
                /// <returns>A message describing the result of the operation.</returns>
                Task AdduserAsync(AddUserRequest adduser);
        }
}
