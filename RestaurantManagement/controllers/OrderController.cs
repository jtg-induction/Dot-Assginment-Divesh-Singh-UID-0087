using RestaurantManagement.Constants;
using RestaurantManagement.Helper;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Enum;
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
    [RoutePrefix("api/order")]
    public class OrderController :ApiController
    {
        private readonly OrderService _orderService;
        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IHttpActionResult> AddOrder(AddOrderRequest addorder)
        {
            int userid =await  ClaimHelper.GetUserIdFromClaim(User.Identity);
           var data= await _orderService.AddOrder(addorder.ItemAndQuantity,addorder.AddressId,userid);
        

            var response = new BaseResponse<OrderResponse>()
            {
                success = true,
                message =ValidationMessages.OrderPlaced,
                data = data

            };
         
            return Created("", response);
        }
    }
}