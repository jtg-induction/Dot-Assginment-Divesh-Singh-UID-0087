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
        Task AddAddressAysnc(Address address);
        Task<Address> GetAddress(int id);
    }
}