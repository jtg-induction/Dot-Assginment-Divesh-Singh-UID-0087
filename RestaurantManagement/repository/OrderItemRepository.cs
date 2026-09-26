using Kendo.Mvc.Extensions;
using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantManagement.Repository
{
    public class OrderItemRepository:IOrderItemRepository
    {
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used to access user records.</param>
        public OrderItemRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public async Task AddOrderItem(List<OrderItem> orderItems)
        {
            _db.OrderItems.AddRange(orderItems);
            await _db.SaveChangesAsync();
        }
        public async Task<List<OrderItem>> GetOrderItem(int id)
        {
            return await _db.OrderItems.Where(e => e.OrderId == id).ToListAsync();
        }
        public async Task<Dictionary<int,int>> GetOrderItemAsItemIdAndQuantity(int id)
        {
            return await _db.OrderItems.Where(e => e.OrderId == id).ToDictionaryAsync(e => e.ItemId, e => e.Quantity);
        }
    }
}