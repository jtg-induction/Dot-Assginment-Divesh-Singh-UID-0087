using RestaurantManagement.Constants;
//using OWIN.WebApi.Controllers;
//using RestaurantManagement.Common;
using RestaurantManagement.Models;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Services;
//using RestaurantManagement.services;
using RestaurantManagement.Services;
using RestaurantManagement.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
//using RestaurantManagement.services;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace RestaurantManagement.Controllers
{
    /// <summary>
    /// Provides authentication-related API endpoints.
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IObtainJwtService _jwtClaim;
        public AuthController(IUserService userser, IObtainJwtService jwtclaim, ITokenService tokenService)
        {
            _userService = userser;
            _jwtClaim = jwtclaim;
            _tokenService = tokenService;
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
            await _userService.AdduserAsync(adduser);
            return Ok(ValidationMessages.Success);
        }
        [HttpPost]
        [Route("login")]
        public async Task<IHttpActionResult> Login(UserCredential login)
        {
            var user = await _userService.LoginUserAsync(login);
            var refreshtoken = await _tokenService.AddRefreshTokenAsync(user.UserId);
            var accesstoken = _jwtClaim.CraftJwt(user);
            _tokenService.SetRefreshTokenCookie(refreshtoken);
            return Ok(new { AccessToken = accesstoken });
        }
        [HttpPost]
        [Route("logout")]
        public async Task<IHttpActionResult> Logout()
        {
            string currentRefreshToken = _tokenService.GetRefreshTokenFromCookie();
            await _tokenService.RevokedAsync(currentRefreshToken);
            _tokenService.ClearRefreshTokenCookie();
            return Ok(ValidationMessages.Success);
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IHttpActionResult> Refresh()
        {
            string token = _tokenService.GetRefreshTokenFromCookie();
            var refreshtoken = await _tokenService.RefreshTheTokenAsync(token);
            var tokenDetail = await _tokenService.GetTokenDetailAsync(refreshtoken);
            var user = await _userService.GetUserAsync(tokenDetail.UserId);
            var accesstoken = _jwtClaim.CraftJwt(user);
            _tokenService.SetRefreshTokenCookie(refreshtoken);
            return Ok(new { AccessToken = accesstoken });

        }
        [HttpPut]
        [Route("deactivate")]
        public async Task<IHttpActionResult> DeactivateAccount(UserCredential login)
        {

            await _userService.DeactivateAccount(login);
            return Ok(ValidationMessages.Success);


        }
        [HttpPut]
        [Route("activate")]
        public async Task<IHttpActionResult> ActivateAccount(UserCredential login)
        {

            await _userService.ActivateAccount(login);
            return Ok(ValidationMessages.Success);


        }


    }
}

