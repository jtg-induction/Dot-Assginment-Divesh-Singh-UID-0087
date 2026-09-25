using RestaurantManagement.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantManagement.Repository.Interface
{
    public interface IAddressRepository
    {
        Task AddAddressAsync(Address address);
        Task<Address> GetAddressAsync(int id, int userid);
        Task<Address> GetAddressAsync(int id);
    }
}