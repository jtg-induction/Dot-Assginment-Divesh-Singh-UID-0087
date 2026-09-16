using RestaurantManagement.Constants;
using RestaurantManagement.Exceptions;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.repository;
using RestaurantManagement.services;
using RestaurantManagement.Services.Interface;
using System.Threading.Tasks;

namespace RestaurantManagement.Services
{
	/// <summary>
	/// Provides user registration services.
	/// </summary>
	public class UserService : IUserService
	{
		private readonly IUserRepository _userrepository;
        private readonly IPasswordService _passwordHasher;

		/// <summary>
		/// Initializes a new instance of the <see cref="UserService"/> class.
		/// </summary>
		/// <param name="userrepository">The repository used to store and validate users.</param>
		/// <param name="passwordHasher">The service used to hash passwords.</param>
		public UserService(IUserRepository userrepository, IPasswordService passwordHasher) {
			_userrepository = userrepository;
			_passwordHasher = passwordHasher;
		}

		/// <summary>
		/// Registers a new customer when the email address and phone number are unique.
		/// </summary>
		/// <param name="adduser">The details of the user to register.</param>
		/// <returns>A validation message describing the registration result.</returns>
		public async Task<User> AdduserAsync(AddUserRequest adduser)
		{
            if (await _userrepository.EmailExistsAsync(adduser.Email))
                throw new ResourceException(ValidationMessages.DuplicateEmail);

            if (await _userrepository.PhoneNumberExistsAsync(adduser.PhoneNumber))
                throw new ResourceException(ValidationMessages.DuplicatePhone);

            var userentity = new User()
					{
						Name = adduser.Name,
						Password =_passwordHasher.HashPassword(adduser.Password),
						Email = adduser.Email,
						BirthDate = adduser.BirthDate,
						PhoneNumber = adduser.PhoneNumber,
						Role = UserRole.Customer
					};
					await _userrepository.AddUserAsync(userentity);
			return userentity;
				
			}


        }
	}

