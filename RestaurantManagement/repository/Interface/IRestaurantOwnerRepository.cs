using RestaurantManagement.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Repository.Interface
{
    public interface IRestaurantOwnerRepository
    {
        Task AddRestaurantOwner(List<RestaurantOwner> restaurantOwners);
    }
}
