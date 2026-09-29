using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Services.Interface
{
        public interface IOrderService
        {
                Task<OrderResponse> AddOrder(AddOrderRequest addOrder, int userid);
                Task<List<GetOrderResponse>> GetOrder(int id);
        Task<List<GetOrderItemResponse>> GetOrderItem(int id, int userid);
                Task OrderCancel(int id, int userid);
                Task<GetPaginatedResponse<GetOrderResponseForOwner>> GetAllOrder(OrderRequestForOwner orderRequestForOwner, int id);
                Task UpdateStatus(UpdateOrderStatusRequest updateOrderStatus, int userid);
        }
}
