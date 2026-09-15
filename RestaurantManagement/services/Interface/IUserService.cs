using RestaurantManagement.Models.Dto;
using System.Threading.Tasks;
using RestaurantManagement.Models;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;

namespace RestaurantManagement.Services
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
                string Adduser(AddUserRequest adduser);
                User CheckUser(UserCredential usercr);
                User GetUser(int id);
        }
}
