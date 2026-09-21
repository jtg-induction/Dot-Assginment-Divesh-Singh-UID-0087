using RestaurantManagement.Constants;
using RestaurantManagement.Helper;
using RestaurantManagement.Models;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Services;
using RestaurantManagement.Services;
using RestaurantManagement.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Models.Entity;
using System.Web.Http;
using Castle.DynamicProxy.Generators;

namespace RestaurantManagement.Controllers
{
    /// <summary>
    /// Provides authentication-related API endpoints.
    /// </summary>
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        private readonly IObtainJwtService _jwtClaim;
        private readonly IClaimHelper _claimHelper;

        public AuthController(IUserService userservice, IObtainJwtService jwtclaim, ITokenService tokenService, IClaimHelper claimHelper)
        {
            _userService = userservice;
            _jwtClaim = jwtclaim;
            _tokenService = tokenService;
            _claimHelper = claimHelper;
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
            var response = new BaseResponse<string>
            {
                success = true,
                message = ValidationMessages.UserCreated
            };
            return Created(string.Empty, response);
        }
        [HttpPost]
        [Route("login")]
        public async Task<IHttpActionResult> Login(UserCredential login)
        {
            var user = await _userService.LoginUserAsync(login);
            var refreshtoken = await _tokenService.AddRefreshTokenAsync(user.UserId);
            string accesstoken = _jwtClaim.CraftJwt(user);
            _tokenService.SetRefreshTokenCookie(refreshtoken);
            var responseData = new LoginResponse()
            {
                Token = accesstoken
            };
            var response = new BaseResponse<LoginResponse>
            {
                success = true,
                message = ValidationMessages.LoginSuccess,
                data = responseData
            };
            return Ok(response);
        }
        [Authorize]
        [HttpPost]
        [Route("logout")]
        public async Task<IHttpActionResult> Logout()
        {
            string currentRefreshToken = _tokenService.GetRefreshTokenFromCookie();
            await _tokenService.RevokedAsync(currentRefreshToken);
            _tokenService.ClearRefreshTokenCookie();
            var response = new BaseResponse<string>
            {
                success = true,
                message = ValidationMessages.LogoutSucess
            };
            return Ok(response);
        }
        [HttpPost]
        [Route("refresh")]
        public async Task<IHttpActionResult> Refresh()
        {
            string token = _tokenService.GetRefreshTokenFromCookie();
            var refreshtoken = await _tokenService.RefreshTheTokenAsync(token);
            var tokenDetail = await _tokenService.GetTokenDetailAsync(refreshtoken);
            User user = await _userService.GetUserAsync(tokenDetail.UserId);
            var accesstoken = _jwtClaim.CraftJwt(user);
            _tokenService.SetRefreshTokenCookie(refreshtoken);
            var responseData = new LoginResponse()
            {
                Token = accesstoken
            };
            var response = new BaseResponse<LoginResponse>
            {
                success = true,
                message = ValidationMessages.RefreshSuccess,
                data = responseData
            };
            return Ok(response);

        }
        [Authorize]
        [HttpDelete]
        [Route("deactivate")]
        public async Task<IHttpActionResult> DeactivateAccount()
        {
            int currentUserId = await _claimHelper.GetUserIdFromClaim(User.Identity);
            await _userService.DeactivateAccount(currentUserId);
            var response = new BaseResponse<string>
            {
                success = true,
                message = ValidationMessages.DeactivateSuccess
            };
            return Ok(response);


        }
        [HttpPut]
        [Route("activate")]
        public async Task<IHttpActionResult> ActivateAccount(UserCredential login)
        {

            var user = await _userService.ActivateAccount(login);
            var response = new BaseResponse<string>
            {
                success = true,
                message = ValidationMessages.ActivateSuccess
            };
            return Ok(response);


        }


    }
}
