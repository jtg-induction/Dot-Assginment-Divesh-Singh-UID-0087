using RestaurantManagement.Constants;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace RestaurantManagement.Controllers
{
    [Authorize]
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private readonly UserService _userservice;
        public AccountController(UserService userservice)
        {
            _userservice = userservice;
        }
        [HttpPut]
        [Route("update")]
        public async Task<IHttpActionResult> UpdateAccount(UpdateAccountDto model)
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var userIdClaim = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int currentUserId))
            {
                return Unauthorized();
            }
            var user = await _userservice.GetUserIfActive(currentUserId);

          await  _userservice.UpdateAccount(user, model);
            return Ok(ValidationMessages.Success);

        }
       

    }
}