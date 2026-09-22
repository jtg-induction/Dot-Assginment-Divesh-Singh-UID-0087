using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Repository;
using RestaurantManagement.Repository.Interface;
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
        private readonly IUserAddressRepository _userAddressRepository;
        public UserAddressService(IUserAddressRepository userAddressRepository)
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
        public async Task<List<AddressResponse>> GetAddress(int id)
          {
          var address=await  _userAddressRepository.GetUserAddress(id);

            List<AddressResponse> data = new List<AddressResponse>();
            foreach (Address useraddress in address)
            {
                data.Add(new AddressResponse
                {
                    AddressId = useraddress.AddressId,
                    Street = useraddress.Street,
                    City = useraddress.City,
                    State = useraddress.State,
                    PinCode = useraddress.PinCode,
                    Country = useraddress.Country,
                    AddressType = useraddress.AddressType.ToString()
                });
            }
            return data;
        }
    }
}