using RestaurantManagement.Constants;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;

//using RestaurantManagement.Models.Entity;
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
//using System.Web.Http.Results;

namespace RestaurantManagement.Controllers
{
    [Authorize]
    [RoutePrefix("api/restaurant")]
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
        [Route("activerestaurant")]
        public async Task<IHttpActionResult> ActiveRestaurant()
        {
            var activerestaurant = await _restaurantService.GetRestaurantsAsync();
            var data = new List<ActiveRestaurantResponse>();
            foreach (Restaurant i in activerestaurant)
            {
                data.Add(new ActiveRestaurantResponse
                {
                    RestaurantId = i.RestaurantId,
                    Name = i.Name,
                    AddressId = i.AddressId,
                    Email = i.Email,
                    PhoneNumber = i.PhoneNumber

                });
            }
            var response = new BaseResponse<List<ActiveRestaurantResponse>>()
            {
                success = true,
                message = ValidationMessages.ListOfRestaurant,
                data = data
            };
            return Ok(response);
        }
        [HttpGet]
        [Route("menu/{id}")]
        public async Task<IHttpActionResult> MenuOfRestaurant(int id)
        {
            var data = await _menuService.GetMenuItemsAsync(id);

            var response = new BaseResponse<List<GetMenuItemResponse>>()
            {
                success = true,
                message = ValidationMessages.ListOfMenuItem,
                data = data
            };
            return Ok(response);
        }
    }
}