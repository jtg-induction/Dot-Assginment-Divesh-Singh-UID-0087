using RestaurantManagement.Models.Dto;
using System.Web.Http;
using RestaurantManagement.services;
using System.Threading.Tasks;
using RestaurantManagement.Constants;
using RestaurantManagement.Services;

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
            await _userservice.AdduserAsync(adduser);
            return Ok(ValidationMessages.succes);
             }
        }
    }

