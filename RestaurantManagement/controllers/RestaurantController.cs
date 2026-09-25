using RestaurantManagement.Constants;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Services;
using RestaurantManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
namespace RestaurantManagement.Controllers
{
    [Authorize]
    [RoutePrefix("api/restaurants")]
    public class RestaurantController : ApiController
    {
        private readonly IRestaurantService _restaurantService;
        private readonly IMenuService _menuService;

        public RestaurantController(IRestaurantService restaurantService, IMenuService menuService)
        {
            _restaurantService = restaurantService;
            _menuService = menuService;
        }
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetRestaurants()
        {
           var Data = await _restaurantService.GetRestaurantsAsync();
           
            var response = new BaseResponse<List<ActiveRestaurantResponse>>()
            {
                success = true,
                message = ValidationMessages.ListOfRestaurant,
                data = Data
            };
            return Ok(response);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> GetMenuByRestaurantId(int id)
        {
            if (id <= 0)
            {
                throw new InvalidOperationException(ValidationMessages.InvalidRestaurantId);
            }
            var Data = await _menuService.GetMenuItemsAsync(id);

            var response = new BaseResponse<List<GetMenuItemResponse>>()
            {
                success = true,
                message = ValidationMessages.ListOfMenuItem,
                data = Data
            };
            return Ok(response);
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> AddRestaurant(AddRestaurantRequest addRestaurant)
        {
            await _restaurantService.AddRestaurant(addRestaurant);
            var response = new BaseResponse<string>
            {
                success = true,
                message = ValidationMessages.RestaurantCreatedSuccess
            };
            return Created(string.Empty, response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("owners")]
        public async Task<IHttpActionResult> AddRestaurantOwner(AddRestaurantOwnerRequest addRestaurantOwnerRequest)
        {
            await _restaurantService.AddRestaurantowner(addRestaurantOwnerRequest);
            var response = new BaseResponse<string>
            {
                success = true,
                message = ValidationMessages.RestaurantOwnerUpdateSuccess
            };
            return Ok( response);
        }


    } 
}