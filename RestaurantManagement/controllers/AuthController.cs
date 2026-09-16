using RestaurantManagement.Models.Dto;
using System.Web.Http;
using RestaurantManagement.services;
using System.Threading.Tasks;
using RestaurantManagement.Constants;
using RestaurantManagement.Services;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Models.Entity;

namespace RestaurantManagement.Controllers
{
    /// <summary>
    /// Provides authentication-related API endpoints.
    /// </summary>
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IUserService _userservice;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="userservice">The user service used to manage users.</param>
        public AuthController(IUserService userserice)
        {
            _userservice = userserice;
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="adduser">The new user's registration details.</param>
        /// <returns>The result of the registration request.</returns>
        [HttpPost]
        [Route("signup")]
        public async Task<IHttpActionResult> Signup(AddUserRequest adduser)
        {
           User user =await _userservice.AdduserAsync(adduser);
            var createdresponse = new CreatedUserResponse
            {
                UserId=user.UserId,
                Name=user.Name,
                Email=user.Email,
                PhoneNumber=user.PhoneNumber,
                BirthDate=user.BirthDate,
                CreatedAt=user.CreatedAt,
                UpdatedAt=user.UpdatedAt
            };
            var response = new BaseResponse<CreatedUserResponse>
            {
                success = true,
                message = ValidationMessages.UserCreated,
                data = createdresponse
            };
            return  Created(string.Empty, response);
        }
        }
    }

