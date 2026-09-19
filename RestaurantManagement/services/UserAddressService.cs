using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Repository;
using RestaurantManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantManagement.Services
{
    public class UserAddressService : IUserAddressService
    {
        private readonly UserAddressRepository _userAddressRepository;
        public UserAddressService(UserAddressRepository userAddressRepository)
        {
            _userAddressRepository = userAddressRepository;
        }
       
        public async Task AddUserAdress(int userid,int addressid)
        {
            var adduseraddress = new UserAddress
            {
                UserId = userid,
                AddressId = addressid
            };
            await _userAddressRepository.AddUserAddressAysnc(adduseraddress);
        }
        public async Task<List<Address>> GetAddress(int id)
          {
          var address=await  _userAddressRepository.GetUserAddress(id);

            return address;
        }
    }
}