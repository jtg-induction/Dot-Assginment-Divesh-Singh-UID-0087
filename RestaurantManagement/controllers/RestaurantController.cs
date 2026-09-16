using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Services;
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
    public class RestaurantController :ApiController
    {
        private readonly RestaurantService _restaurantService;
        private readonly MenuService _menuService;

        public RestaurantController(RestaurantService restaurantService,MenuService menuService)
        {
            _restaurantService = restaurantService;
            _menuService = menuService;
        }
        [HttpGet]
        [Route("activerestaurant")]
        public async Task<IHttpActionResult> ActiveRestaurant()
        {
            return Ok(await _restaurantService.GetRestaurantsAsync());
        }
        [HttpGet]
        [Route("menu")]
        public async Task<IHttpActionResult> MenuOfRestaurant(RestaurantMenuRequest restaurant)
        {
            return Ok(await _menuService.GetMenuItemsAsync(restaurant.RestaurantId));
        }
    }
}