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
        Task<OrderResponse> AddOrder(Dictionary<int, int> item, int addressid, int userid);
        Task<List<Order>> GetOrder(int id);
        Task<List<OrderItem>> GetOrderItem(int id);
        Task OrderCancel(int id);
    }
}
