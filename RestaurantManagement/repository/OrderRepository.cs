using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Repository.Interface;
using System;
using System.Collections.Generic;
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
    }
}