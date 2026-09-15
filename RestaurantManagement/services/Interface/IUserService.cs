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
        /// <param name="adduser">The user details to add.</param>
        /// <returns>A message describing the result of the operation.</returns>
        Task<string> AddUserAsync(AddUserRequest addUser);

        Task<User> CheckUserAsync(UserCredential userCredential);
        Task<User> GetUserAsync(int id);
}
}
