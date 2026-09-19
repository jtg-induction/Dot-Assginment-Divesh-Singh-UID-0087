using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
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
    public class AddressService: IAddressService
    {
        private readonly AddressRepository _addressRepository;
        public AddressService(AddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }
        //public async Task<Address> GetAddressAsync(int id)
        //{
        //    return 
        //}
        public async Task<int> AddUserAddress(AddAddressRequest addAddress)
        {
            Address address = new Address()
            {
                Street = addAddress.Street,
                City = addAddress.City,
                State=addAddress.State,
                AddressType=addAddress.AddressType,
                PinCode = addAddress.Pincode,
                Country = addAddress.Country

            };
            await _addressRepository.AddAddressAysnc(address);
            return address.AddressId;
        }
    }
}