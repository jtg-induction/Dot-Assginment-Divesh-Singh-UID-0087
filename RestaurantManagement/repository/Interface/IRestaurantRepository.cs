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
                Task<bool> RestaurantIsActive(int id);


        }
}
