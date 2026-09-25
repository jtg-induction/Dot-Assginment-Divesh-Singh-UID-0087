using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using System.Threading.Tasks;
using RestaurantManagement.Models;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;

namespace RestaurantManagement.Services
{
        public interface IUserService
        {
                /// <summary>
                /// Defines operations for managing users.
                /// </summary>
                /// <param name="adduser">The user details to add.</param>
                /// <returns>A message describing the result of the operation.</returns>
                Task AdduserAsync(AddUserRequest adduser);
                Task<User> LoginUserAsync(UserCredential userCredential);
                Task<User> GetUserAsync(int id);
                Task<User> GetUserIfActive(int id);

                Task UpdateAccount(User user, UpdateAccountDto updateaccount);
                Task DeactivateAccount(int id);
                Task<User> ActivateAccount(UserCredential user);
        }
}
