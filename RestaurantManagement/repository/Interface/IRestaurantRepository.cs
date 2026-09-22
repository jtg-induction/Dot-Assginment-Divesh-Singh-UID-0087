using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Repository.Interface
{
        public interface IRestaurantRepository
        {
                Task<List<Restaurant>> GetRestaurantsAsync();
                Task<string> GetRestaurantName(int id);
                Task AddRestaurant(Restaurant restaurant);
                Task<bool> EmailExixts(string email);
                Task<bool> PhoneNumberExixts(string ph);
                Task<bool> RestaurantIsActive(int id);
        Task<Restaurant> RestaurantIsActive(string email);


        }
}
