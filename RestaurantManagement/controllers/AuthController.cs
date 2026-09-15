using RestaurantManagement.Models.Dto;
using System.Web.Http;
using RestaurantManagement.services;
using System.Threading.Tasks;
using RestaurantManagement.Constants;
using RestaurantManagement.Services;
//using OWIN.WebApi.Controllers;
using RestaurantManagement.Common;
using RestaurantManagement.Models;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
//using RestaurantManagement.services;
using RestaurantManagement.Services;
using RestaurantManagement.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Web.Http;

namespace RestaurantManagement.Controllers
{
    /// <summary>
    /// Provides authentication-related API endpoints.
    /// </summary>
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IUserService _userservice;
        private readonly ITokenService _tokenservice;
        private readonly IObtainJwtService _jwtclaim;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="userservice">The user service used to manage users.</param>
        public AuthController(IUserService userserice)
        {
            _userservice = userserice;
        /// <param name="userser">The user service used to manage users.</param>
            //public AuthController(IUserService userser)
            //{
            //    _userservice = userser;
            //}
        public AuthController(IUserService userser, IObtainJwtService jwtclaim, ITokenService tokenService)
        {
            _userservice = userser;
            _jwtclaim = jwtclaim;
            _tokenservice = tokenService;
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
        [HttpPost]
        [Route("logout")]
        public IHttpActionResult Logout()
        {
            string currentRefreshToken = _tokenservice.GetRefreshTokenFromCookie();
            _tokenservice.ClearRefreshTokenCookie();
            _tokenservice.Revoked(currentRefreshToken);
            return Ok(ValidationMessages.succes);
        }

        [HttpPost]
        [Route("refresh")]
        public IHttpActionResult Refresh()
        {
            string Token = _tokenservice.GetRefreshTokenFromCookie();
            var tokenHandler = new JwtSecurityTokenHandler();
            var refreshtoken = _tokenservice.RefreshTheToken(Token);
            if (refreshtoken.Equals(ValidationMessages.Revoked))
            {
                return Unauthorized();
            }
            var user = _userservice.GetUser(_tokenservice.Gettokendetail(refreshtoken).UserId);
            var accesstoken = _jwtclaim.CraftJwt(user);
            _tokenservice.SetRefreshTokenCookie(refreshtoken);

            return Ok(new { AccessToken = accesstoken });

        }


    }
}
}

