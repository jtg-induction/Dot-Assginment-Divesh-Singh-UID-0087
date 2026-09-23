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
    public class RestaurantOwnerRepository:IRestaurantOwnerRepository
    {
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used to access user records.</param>
        public RestaurantOwnerRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public async Task AddRestaurantOwner(List<RestaurantOwner> restaurantOwners)
        {
            _db.RestaurantOwners.AddRange(restaurantOwners);
           await _db.SaveChangesAsync();
        }
        public async Task<List<int>> GetRestaurantId(int id)
        {
            return await _db.RestaurantOwners.Where(e=>e.UserId==id).Select(e => e.RestaurantId).ToListAsync();
        }
    }
}