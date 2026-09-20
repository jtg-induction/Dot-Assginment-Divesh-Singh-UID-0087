using RestaurantManagement.Constants;
using RestaurantManagement.Helper;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
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
        private readonly RestaurantService _restaurantService;
        public OrderController(OrderService orderService,RestaurantService restaurantService)
        {
            _orderService = orderService;
            _restaurantService = restaurantService;
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
        [Authorize]
        [HttpGet]
        [Route("get")]
        public async Task<IHttpActionResult> GetOrderDetail()
        {
            int userid =await  ClaimHelper.GetUserIdFromClaim(User.Identity);
            var order = await _orderService.GetOrder(userid);
            var data = new List<GetOrderResponse>();
            foreach(Order i in order)
            {
                data.Add(new GetOrderResponse
                {
                    OrderId=i.OrderId,
                    RestaurantName= await _restaurantService.GetRestaurantName(i.RestaurantId),
                    TotalAmount=i.TotalAmount,
                    Address=i.Address,
                    Status=i.Status.ToString()

                });
            }
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
        [Route("get/{id}")]
        public async Task<IHttpActionResult> GetOrderItemDetail(int id)
        {
            var order = await _orderService.GetOrderItem(id);
            var data = new List<GetOrderItemResponse>();
            foreach (OrderItem i in order)
            {
                data.Add(new GetOrderItemResponse
                {
                    OrderItemId=i.OrderItemId,
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    Price = i.Price,
                    Quantity = i.Quantity

                });
            }
            var response = new BaseResponse<List<GetOrderItemResponse>>
            {
                success = true,
                message = ValidationMessages.OrderFetchSuccess,
                data = data
            };
            return Ok(response);
        }
        [Authorize (Roles ="Customer")]
        [HttpPut]
        [Route("cancel/{id}")]
        public async Task<IHttpActionResult> CancelOrder(int id)
        {
          
           await _orderService.OrderCancel(id);
            var response = new BaseResponse<string>
            {
                success = true,
                message=ValidationMessages.OrderCancelSuccess
            };
            return Ok(response);


        }
    }
}