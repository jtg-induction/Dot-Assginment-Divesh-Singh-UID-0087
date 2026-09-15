using RestaurantManagement.Models.Dto;
using RestaurantManagement.Constants;
using RestaurantManagement.Services;
using RestaurantManagement.Models;
using RestaurantManagement.Services.Interface;
using System.Web.Http;
using System.Threading.Tasks;

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
        [Route("login")]
        public async Task<IHttpActionResult> Login(UserCredential login)
        {

            var user = await _userService.CheckUserAsync(login);

            if (user != null)
            {

                var tokenHandler = new JwtSecurityTokenHandler();
                var refreshtoken = await _tokenService.AddRefreshTokenAsync(user.UserId);
                var accesstoken = _jwtClaim.CraftJwt(user);

                _tokenService.SetRefreshTokenCookie(refreshtoken);

                return Ok(new { AccessToken = accesstoken });
            }

            return Unauthorized();
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


    }
}

