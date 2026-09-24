using RestaurantManagement.Constants;
using RestaurantManagement.Helper;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Services;
using RestaurantManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace RestaurantManagement.Controllers
{
     [Authorize]
    [RoutePrefix("api/orders")]
    public class OrderController : ApiController
    {
        private readonly IOrderService _orderService;
        private readonly IClaimHelper _claimHelper;
        public OrderController(IOrderService orderService, IClaimHelper claimHelper)
        {
            _orderService = orderService;
            _claimHelper = claimHelper;
        }
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> AddOrder(AddOrderRequest addorder)
        {
            int userid = await _claimHelper.GetUserIdFromClaim(User.Identity);
            var data = await _orderService.AddOrder(addorder, userid);
            var response = new BaseResponse<OrderResponse>()
            {
                success = true,
                message = ValidationMessages.OrderPlaced,
                data = data

            };

            return Created("", response);
        }
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetOrder()
        {
            int userid = await _claimHelper.GetUserIdFromClaim(User.Identity);
            var data = await _orderService.GetOrder(userid);
            var response = new BaseResponse<List<GetOrderResponse>>
            {
                success = true,
                message = ValidationMessages.OrderFetchSuccess,
                data = data
            };
            return Ok(response);
        }
        [HttpGet]
        [Route("{id}/order-items")]
        public async Task<IHttpActionResult> GetOrderItem(int id)
        {
            if (id <= 0)
            {
                throw new InvalidOperationException(ValidationMessages.InvalidOrderId);
            }
            var data = await _orderService.GetOrderItem(id);

            var response = new BaseResponse<List<GetOrderItemResponse>>
            {
                success = true,
                message = ValidationMessages.OrderFetchSuccess,
                data = data
            };
            return Ok(response);
        }
        [HttpPut]
        [Route("{id}/cancel")]
        public async Task<IHttpActionResult> CancelOrder(int id)
        {
            if (id <= 0)
            {
                throw new InvalidOperationException(ValidationMessages.InvalidOrderId);
            }
            int userid = await _claimHelper.GetUserIdFromClaim(User.Identity);
            await _orderService.OrderCancel(id, userid);
            var response = new BaseResponse<string>
            {
                success = true,
                message = ValidationMessages.OrderCancelSuccess
            };
            return Ok(response);


        }
    }
}