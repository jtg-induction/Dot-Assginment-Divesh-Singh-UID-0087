using RestaurantManagement.Models.Entity;
using System;
using System.Threading.Tasks;

namespace RestaurantManagement.repository
{
        /// <summary>
        /// Defines data access operations for users.
        /// </summary>
        public interface IUserRepository
        {
                /// <summary>
                /// Determines whether an email address is already registered.
                /// </summary>
                /// <param name="email">The email address to check.</param>
                /// <returns><see langword="true"/> if the email exists; otherwise, <see langword="false"/>.</returns>
                Task<bool> EmailExistsAsync(string email);

                /// <summary>
                /// Determines whether a phone number is already registered.
                /// </summary>
                /// <param name="phoneNumber">The phone number to check.</param>
                /// <returns><see langword="true"/> if the phone number exists; otherwise, <see langword="false"/>.</returns>
                Task<bool> PhoneNumberExistsAsync(string phoneNumber);

                /// <summary>
                /// Adds a user to the data store.
                /// </summary>
                /// <param name="userentity">The user to add.</param>
                /// <returns>The identifier of the added user.</returns>
                Task AddUserAsync(User userentity);

                /// <summary>
                /// Retrieves a user by email address.
                /// </summary>
                /// <param name="email">The email address of the user.</param>
                /// <returns>The matching user.</returns>
                Task<User> GetUserAsync(string email);

                Task<User> GetUserAsync(int id);
                Task<bool> IsActiveAsync(int id);



        }
}