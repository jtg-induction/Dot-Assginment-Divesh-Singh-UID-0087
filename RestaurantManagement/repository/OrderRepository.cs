using NMemory.Linq;
using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantManagement.Repository
{
    public class OrderRepository :IOrderRepository
    {
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used to access user records.</param>
        public OrderRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public async Task PlacedOrder(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
        }
        public async Task<List<Order>> GetOrder(int id)
        {
            return await _db.Orders.Include(e=>e.Restaurant).Where(e => e.UserId == id).ToListAsync();
        }
        public async Task<Order> GetOrderDetail(int id)
        {
            return await _db.Orders.FindAsync(id);
        }
       public async Task CancelOrder(Order order)
        {
            order.Status = OrderStatus.Cancelled;
            await _db.SaveChangesAsync();
        }
    }
}