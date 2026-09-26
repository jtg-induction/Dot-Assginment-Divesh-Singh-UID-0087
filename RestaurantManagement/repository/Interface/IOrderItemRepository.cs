using RestaurantManagement.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Repository.Interface
{
    public interface IOrderItemRepository
    {
        Task AddOrderItem(List<OrderItem> orderItems);
        Task<List<OrderItem>> GetOrderItem(int id);
        Task<Dictionary<int, int>> GetOrderItemAsItemIdAndQuantity(int id);
    }
}
