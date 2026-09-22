using RestaurantManagement.Constants;
using RestaurantManagement.Helper;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Services;
using RestaurantManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace RestaurantManagement.Controllers
{
    [RoutePrefix("api/address")]
    public class AddressController:ApiController
    {
        private readonly IAddressService _addressService;
        private readonly IUserAddressService _userAddressService;
        private readonly IClaimHelper _claimHelper;
        public AddressController( IAddressService addressService, IUserAddressService userAddressService,IClaimHelper claimHelper)
        {
            _addressService = addressService;
            _userAddressService = userAddressService;
            _claimHelper = claimHelper;
        }


        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IHttpActionResult> AddAddress(AddAddressRequest addAddress)
        {
            int userid = await _claimHelper.GetUserIdFromClaim(User.Identity);
            int id = await _addressService.AddUserAddress(addAddress);
            await _userAddressService.AddUserAdress(userid, id);
            var response = new BaseResponse<string>
            {
                success = true,
                message = ValidationMessages.AddresSuccess
            };
            return Ok(response);
        }
        [Authorize]
        [HttpGet]
        [Route("get")]
        public async Task<IHttpActionResult> GetUserAddress()
        {
            int userid = await _claimHelper.GetUserIdFromClaim(User.Identity);
            var data = await _userAddressService.GetAddress(userid);
          
            var response = new BaseResponse<List<AddressResponse>>
            {
                success = true,
                message = ValidationMessages.AddressFetchSuccess,
                data = data
            };
            return Ok(response);
        }

    }
}