using Microsoft.Owin.BuilderProperties;
using RestaurantManagement.Constants;
using RestaurantManagement.Helper;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
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
    [RoutePrefix("api/accounts")]
    public class AccountController : ApiController
    {
        private readonly IUserService _userservice;
        private readonly IClaimHelper _claimHelper;
        public AccountController(IUserService userservice, IClaimHelper claimHelper)
        {
            _userservice = userservice;
            _claimHelper = claimHelper;
        }
        [HttpPut]
        [Route("update")]
        public async Task<IHttpActionResult> UpdateAccount(UpdateAccountDto model)
        {
            int currentUserId = await _claimHelper.GetUserIdFromClaim(User.Identity);
            var user = await _userservice.GetUserIfActive(currentUserId);
            await _userservice.UpdateAccount(user, model);
            var response = new BaseResponse<string>
            {
                success = true,
                message = ValidationMessages.UpdateSuccess
            };
            return Ok(response);

        }


    }
}