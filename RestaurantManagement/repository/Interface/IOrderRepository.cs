using RestaurantManagement.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Repository.Interface
{
    public interface IOrderRepository
    {
        Task PlacedOrder(Order order);
        Task<List<Order>> GetOrder(int id);
        Task CancelOrder(Order order);
        Task<Order> GetOrderDetail(int id);
    }
}
