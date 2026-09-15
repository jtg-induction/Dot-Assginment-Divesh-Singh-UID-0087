using RestaurantManagement.Constants;
using RestaurantManagement.Exceptions;
using RestaurantManagement.Models;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.repository;
using RestaurantManagement.Repository;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services.Interface;
using System;
using System.CodeDom;
using System.Threading.Tasks;

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
				throw new ResourceException(ValidationMessages.DuplicateEmail);

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
			if (!await _userrepository.IsActiveAsync(user.UserId) || user == null)
			{
				throw new ResourceException(ValidationMessages.NotFound);

			}

			if (!_passwordService.VerifyPassword(userCredential.Password, user.Password))
			{
				throw new ResourceException(ValidationMessages.NotFound);
			}
			return user;
		}
		public async Task<User> GetUserAsync(int id)
		{
			return await _userrepository.GetUserAsync(id);
		}

	}
public async Task<User> GetUserAsync(int id)
		{
			return await _userrepository.GetUserAsync(id);
		}

		public async Task<User> GetUserIfActive(int id)
		{
			if (await _userrepository.IsActiveAsync(id))
			{
				return await _userrepository.GetUserAsync(id);
			}
			throw new ResourceException(ValidationMessages.NotFound);

		}

		public async Task UpdateAccount(User user, UpdateAccountDto updateaccount)
		{
			if (user.Email != updateaccount.Email && await _userrepository.EmailExistsOtherThanThisIdAsync(updateaccount.Email, user.UserId))
			{
				throw new ResourceException(ValidationMessages.DuplicateEmail);

			}

			if (user.PhoneNumber != updateaccount.PhoneNumber && await _userrepository.PhoneNumberExistsOtherThanThisIdAsync(updateaccount.PhoneNumber, user.UserId))
			{
				throw new ResourceException(ValidationMessages.DuplicatePhone);

			}

			user.Name = updateaccount.Name;
			user.Email = updateaccount.Email;
			user.PhoneNumber = updateaccount.PhoneNumber;
			user.BirthDate = updateaccount.BirthDate.Date;
			user.UpdatedAt = DateTime.UtcNow;

			await _userrepository.UpdateAccount(user);

		}
	}
}
