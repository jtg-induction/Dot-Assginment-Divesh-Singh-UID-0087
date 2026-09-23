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
        [Authorize]
        [HttpPost]
        [Route("add")]
        public async Task<IHttpActionResult> AddOrder(AddOrderRequest addorder)
        {
            int userid = await _claimHelper.GetUserIdFromClaim(User.Identity);
            var data = await _orderService.AddOrder(addorder.ItemAndQuantity, addorder.AddressId, userid);
            var response = new BaseResponse<GetOrderResponse>()
            {
                success = true,
                message = ValidationMessages.OrderPlaced,
                data = data

            };

            return Created("", response);
        }
        [Authorize]
        [HttpGet]
        [Route("get")]
        public async Task<IHttpActionResult> GetOrderDetail()
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
        [Authorize]
        [HttpGet]
        [Route("{id}/items")]
        public async Task<IHttpActionResult> GetOrderItemDetail([FromUri] int id)
        {
            var data = await _orderService.GetOrderItem(id);

            var response = new BaseResponse<List<GetOrderItemResponse>>
            {
                success = true,
                message = ValidationMessages.OrderFetchSuccess,
                data = data
            };
            return Ok(response);
        }
        [Authorize(Roles = "Customer")]
        [HttpPut]
        [Route("{id}/cancel")]
        public async Task<IHttpActionResult> CancelOrder([FromUri] int id)
        {
            int userid = await _claimHelper.GetUserIdFromClaim(User.Identity);
            await _orderService.OrderCancel(id, userid);
            var response = new BaseResponse<string>
            {
                success = true,
                message = ValidationMessages.OrderCancelSuccess
            };
            return Ok(response);
        }
        [Authorize(Roles ="Owner")]
        [HttpGet]
        [Route]
        public async Task<IHttpActionResult> GetAllOrder([FromUri]PaginationParams paginationParams)
        {
            if (paginationParams.pageNumber < 1) paginationParams.pageNumber = 1;
            if (paginationParams.pageSize < 1 ) paginationParams.pageSize= 10;
            var userId = await _claimHelper.GetUserIdFromClaim(User.Identity);
            var data=await _orderService.GetAllOrder(paginationParams,userId);
            var response = new BaseResponse<GetPaginatedResponse<GetOrderResponseForOwner>>
            {
                success = true,
                message = ValidationMessages.OrderFetchSuccess,
                data = data
            };
            return Ok(response);
        }
        [Authorize(Roles ="Owner")]
        [HttpPut]
        [Route("update-status")]
        public async Task<IHttpActionResult> UpdateOrderStatus(UpdateOrderStatusRequest updateOrderStatus)
        {
           await  _orderService.UpdateStatus(updateOrderStatus);
            var response = new BaseResponse<string>
            {
                success = true,
                message = ValidationMessages.OrderStatusUpdate
            };
            return Ok(response);
        }
    }
}