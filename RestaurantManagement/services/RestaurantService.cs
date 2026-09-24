using Microsoft.Owin.BuilderProperties;
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
        private readonly IAddressRepository _addressRepository;
        public RestaurantService(IRestaurantRepository restaurantRepository,IAddressRepository addressRepository)
        {
            _restaurantRepository = restaurantRepository;
            _addressRepository = addressRepository;
        }
        public async Task<List<ActiveRestaurantResponse>> GetRestaurantsAsync()
        {
            var activeRestaurants = await _restaurantRepository.GetRestaurantsAsync();
           var data1 = new List<ActiveRestaurantResponse>();
            foreach (Restaurant restaurant in activeRestaurants)
            {
               var data= await _addressRepository.GetAddressAsync(restaurant.AddressId);
                string address = $"{data.Street},{data.City},{data.State},{data.Country},{data.PinCode},{data.AddressType}";
                data1.Add(new ActiveRestaurantResponse
                {
                    RestaurantId = restaurant.RestaurantId,
                    Name = restaurant.Name,
                    Address=address,
                    Email = restaurant.Email,
                    PhoneNumber = restaurant.PhoneNumber

                });
            }
            return data1;
        }
    }
}