using RestaurantManagement.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantManagement.Services.Interface
{
    public interface IAddressService
    {
        Task<int> AddUserAddress(AddAddressRequest addAddress);
    }
}