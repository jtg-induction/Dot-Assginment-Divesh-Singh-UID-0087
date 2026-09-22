using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Repository;
using RestaurantManagement.Repository.Interface;
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
        private readonly IRestaurantRepository _restaurantRepository;
        public RestaurantService(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }
        public async Task<List<ActiveRestaurantResponse>> GetRestaurantsAsync()
        {
            var activeRestaurants = await _restaurantRepository.GetRestaurantsAsync();
            var data = new List<ActiveRestaurantResponse>();
            foreach (Restaurant restaurant in activeRestaurants)
            {
                data.Add(new ActiveRestaurantResponse
                {
                    RestaurantId = restaurant.RestaurantId,
                    Name = restaurant.Name,
                    AddressId = restaurant.AddressId,
                    Email = restaurant.Email,
                    PhoneNumber = restaurant.PhoneNumber

                });
            }
            return data;
        }
    }
}