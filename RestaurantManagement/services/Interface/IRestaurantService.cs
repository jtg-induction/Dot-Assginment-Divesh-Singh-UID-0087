using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Services.Interface
{
    public interface IRestaurantService
    {
        Task<List<Restaurant>> GetRestaurantsAsync();
        Task<string> GetRestaurantName(int id);
        Task AddRestaurant(AddRestaurantRequest addRestaurant);
        Task AddRestaurantowner(AddRestaurantOwnerRequest addRestaurantOwnerRequest);

    }
}
