using RestaurantManagement.Models.Entity;
using RestaurantManagement.Repository;
using RestaurantManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantManagement.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantRepository _restaurantRepository;
        public RestaurantService(RestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }
        public async Task<List<Restaurant>> GetRestaurantsAsync()
        {
            return await _restaurantRepository.GetRestaurantsAsync();
        }
        public async Task<string> GetRestaurantName(int id)
        {
            return await _restaurantRepository.GetRestaurantName(id);
        }
    }
}