using RestaurantManagement.Data;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.repository;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagement.Repository
{
    /// <summary>
    /// Repository for managing user-related data access logic.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used to access user records.</param>
        public UserRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        /// <summary>
        /// Retrieves a user by email address.
        /// </summary>
        /// <param name="email">The email to search for.</param>
        /// <returns>The matching user if found; otherwise, null.</returns>
        //public async Task<string> GetUserEmail(string email)
        //{
        
        //    User user = await _db.Users.FirstAsync(e => e.Email == email);
          
        //    return user.Email;
        //}
        public async Task<User> GetUserAsync(string email)
        {

          
            return await _db.Users.FirstOrDefaultAsync(e => e.Email == email);

        }

        /// <summary>
        /// Checks whether a user with the specified email already exists.
        /// </summary>
        /// <param name="email">The email to validate.</param>
        /// <returns>True if the email exists; otherwise, false.</returns>
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _db.Users.AnyAsync(e => e.Email == email);
        }

        /// <summary>
        /// Checks whether a user with the specified phone number already exists.
        /// </summary>
        /// <param name="phoneNumber">The phone number to validate.</param>
        /// <returns>True if the phone number exists; otherwise, false.</returns>
        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber)
        {
            return await _db.Users.AnyAsync(e => e.PhoneNumber == phoneNumber);
        }

        /// <summary>
        /// Adds a new user to the database.
        /// </summary>
        /// <param name="userentity">The user entity to add.</param>
        /// <returns>The ID of the newly added user.</returns>
        public async Task AddUserAsync(User userentity)
        {
            _db.Users.Add(userentity);
            await _db.SaveChangesAsync();
        }
        public async Task<User> GetUserAsync(int id)
        {
            return await _db.Users.FindAsync(id);
        }
        public async Task<bool> IsActiveAsync(int id)
        {
            var user = await _db.Users.FindAsync(id);
            return user != null && user.IsActive;
        }
      

    

        public async Task UpdateAccount(User user,UpdateAccountDto updateaccount)
        {
            user.Name = updateaccount.Name;
            user.Email = updateaccount.Email;
            user.PhoneNumber = updateaccount.PhoneNumber;
            user.BirthDate = updateaccount.BirthDate;
            user.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }

        public async Task Deactivate(User user)
        {
            user.IsActive = false;
           await _db.SaveChangesAsync();
        }
        public async Task Activate(User user)
        {
            user.IsActive = true;
          await  _db.SaveChangesAsync();
        }
    }
}
