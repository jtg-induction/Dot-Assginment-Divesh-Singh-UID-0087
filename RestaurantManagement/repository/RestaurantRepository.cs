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
        public async Task<bool> EmailExixts(string email)
        {
            return await _db.Restaurants.AnyAsync(e => e.Email == email);
        }
        public async Task<bool> PhoneNumberExixts(string ph)
        {
            return await _db.Restaurants.AnyAsync(e => e.PhoneNumber==ph);
        }
        public async Task<List<Restaurant>> GetRestaurantsAsync()
        {
            return _db.Restaurants.Where(e => e.IsActive).ToList();
        }
        public async Task<string> GetRestaurantName(int id)
        {
            return _db.Restaurants.Where(e => e.RestaurantId == id).Select(e => e.Name).FirstOrDefault();
        }
        public async Task AddRestaurant(Restaurant restaurant)
        {

            _db.Restaurants.Add(restaurant);
            await _db.SaveChangesAsync();
        }
    }

}