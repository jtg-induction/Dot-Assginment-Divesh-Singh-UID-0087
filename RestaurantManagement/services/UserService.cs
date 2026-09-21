using RestaurantManagement.Constants;
using RestaurantManagement.Exceptions;
using RestaurantManagement.Models;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.repository;
using RestaurantManagement.Repository;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services.Exceptions;
using RestaurantManagement.Services.Interface;
using System;
using System.CodeDom;
using System.Threading.Tasks;
using System.Web.WebPages;

namespace RestaurantManagement.Services
{
    /// <summary>
    /// Provides user registration services.
    /// </summary>

    public class UserService : IUserService
    {
        private readonly IUserRepository _userrepository;
        private readonly IPasswordService _passwordService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userrepository">The repository used to store and validate users.</param>
        /// <param name="passwordHasher">The service used to hash passwords.</param>
        public UserService(IUserRepository userrepository, IPasswordService passwordHasher)
        {
            _userrepository = userrepository;
            _passwordService = passwordHasher;

        }

        /// <summary>
        /// Registers a new customer when the email address and phone number are unique.
        /// </summary>
        /// <param name="adduser">The details of the user to register.</param>
        /// <returns>A validation message describing the registration result.</returns>
        public async Task AdduserAsync(AddUserRequest adduser)
        {
            if (await _userrepository.EmailExistsAsync(adduser.Email))
            {
                throw new ResourceException(ValidationMessages.DuplicateEmail);
            }

            if (await _userrepository.PhoneNumberExistsAsync(adduser.PhoneNumber))
                throw new ResourceException(ValidationMessages.DuplicatePhone);

            var userentity = new User()
            {
                Name = adduser.Name,
                Password = _passwordService.HashPassword(adduser.Password),
                Email = adduser.Email,
                BirthDate = adduser.BirthDate,
                PhoneNumber = adduser.PhoneNumber,
                Role = UserRole.Customer
            };
            await _userrepository.AddUserAsync(userentity);


        }
        public async Task<User> LoginUserAsync(UserCredential userCredential)
        {
            User user = await _userrepository.GetUserAsync(userCredential.Email);
            if (user == null || !await _userrepository.IsActiveAsync(user.UserId))
            {
                throw new UnauthenticatedException(ValidationMessages.UserNotFound);

            }

            if (!_passwordService.VerifyPassword(userCredential.Password, user.Password))
            {
                throw new UnauthenticatedException(ValidationMessages.UserNotFound);
            }
            return user;
        }

        public async Task DeactivateAccount(int id)
        {
            User userdetail = await _userrepository.GetUserAsync(id);

            if (userdetail == null)
            {
                throw new UnauthenticatedException(ValidationMessages.UserNotFound);
            }
            await _userrepository.Deactivate(userdetail);


        }
        public async Task<User> ActivateAccount(UserCredential user)
        {
            User userdetail = await _userrepository.GetUserAsync(user.Email);
            if (userdetail == null)
            {
                throw new UnauthenticatedException(ValidationMessages.UserNotFound);
            }
            await _userrepository.Activate(userdetail);
            return userdetail;
        }
        public async Task<User> GetUserAsync(int id)
        {
            return await _userrepository.GetUserAsync(id);
        }

        public async Task<User> GetUserIfActive(int id)
        {
            if (!await _userrepository.IsActiveAsync(id))
            {
                throw new UnauthenticatedException(ValidationMessages.UserNotFound);
            }
            return await _userrepository.GetUserAsync(id);

        }

        public async Task UpdateAccount(User user, UpdateAccountDto updateaccount)
        {
            if (!await _userrepository.IsActiveAsync(user.UserId))
            {
                throw new UnauthenticatedException(ValidationMessages.UserNotFound);

            }
            if (await _userrepository.EmailExistsAsync(updateaccount.Email))
            {
                throw new ResourceException(ValidationMessages.DuplicateEmail);

            }

            if (await _userrepository.PhoneNumberExistsAsync(updateaccount.PhoneNumber))
            {
                throw new ResourceException(ValidationMessages.DuplicatePhone);

            }
            if (updateaccount.Name.IsEmpty())
            {
                updateaccount.Name = user.Name;
            }
            if (updateaccount.Email.IsEmpty())
            {
                updateaccount.Email = user.Email;
            }
            if (updateaccount.PhoneNumber.IsEmpty())
            {
                updateaccount.PhoneNumber = user.PhoneNumber;
            }
            if (updateaccount.BirthDate.Equals(DateTime.MinValue))
            {
                updateaccount.BirthDate = user.BirthDate;
            }

            await _userrepository.UpdateAccount(user, updateaccount);

        }
    }
}