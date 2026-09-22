using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Services.Interface
{
    public interface IUserAddressService
    {
        Task AddUserAdress(int userid, int addressid);
        Task<List<AddressResponse>> GetAddress(int id);
    }
}
