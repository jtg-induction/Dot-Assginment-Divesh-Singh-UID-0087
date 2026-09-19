using RestaurantManagement.Constants;
using RestaurantManagement.Helper;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Services;
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
        private readonly AddressService _addressService;
        private readonly UserAddressService _userAddressService;
        public AddressController( AddressService addressService, UserAddressService userAddressService)
        {
            _addressService = addressService;
            _userAddressService = userAddressService;
        }


        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IHttpActionResult> AddAdress(AddAddressRequest addAddress)
        {
            int userid = await ClaimHelper.GetUserIdFromClaim(User.Identity);
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
            int userid = await ClaimHelper.GetUserIdFromClaim(User.Identity);
            var address = await _userAddressService.GetAddress(userid);
            List<AddressResponse> data = new List<AddressResponse>();
            foreach (Models.Entity.Address i in address)
            {
                data.Add(new AddressResponse
                {
                    AddressId = i.AddressId,
                    Street = i.Street,
                    City = i.City,
                    State=i.State,
                    PinCode = i.PinCode,
                    Country = i.Country,
                    AddressType = i.AddressType.ToString()
                });
            }
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