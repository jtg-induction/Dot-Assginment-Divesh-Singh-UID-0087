using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantManagement.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used to access user records.</param>
        public RestaurantRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public async Task<bool> IsRestaurantActive(int id)
        {
            return await  _db.Restaurants.Where(e => e.RestaurantId == id).Select(e => e.IsActive).FirstOrDefaultAsync();
        }
        public async Task<List<Restaurant>> GetRestaurantsAsync()
        {
            return await  _db.Restaurants.Where(e => e.IsActive).ToListAsync();
        }
        public async Task<string> GetRestaurantName(int id)
        {
            return await _db.Restaurants.Where(e => e.RestaurantId == id).Select(e => e.Name).FirstOrDefaultAsync();
        }
    }

}