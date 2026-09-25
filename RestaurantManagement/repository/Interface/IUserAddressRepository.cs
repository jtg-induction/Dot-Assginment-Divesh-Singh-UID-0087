using RestaurantManagement.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantManagement.Repository.Interface
{
    public interface IUserAddressRepository
    {
        Task<List<Address>> GetUserAddress(int id);
        Task AddUserAddressAysnc(UserAddress userAddress);
    }
}