using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Repository;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace RestaurantManagement.Services
{
    public class AddressService: IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IUserAddressRepository _userAddressRepository;
        public AddressService(IAddressRepository addressRepository,IUserAddressRepository userAddressRepository)
        {
            _addressRepository = addressRepository;
            _userAddressRepository = userAddressRepository;

        }
        public async Task AddUserAddress(AddAddressRequest addAddress,int userid)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                Address address = new Address()
                {
                    Street = addAddress.Street,
                    City = addAddress.City,
                    State = addAddress.State,
                    AddressType = addAddress.AddressType,
                    PinCode = addAddress.Pincode,
                    Country = addAddress.Country

                };
                await _addressRepository.AddAddressAsync(address);
                var adduseraddress = new UserAddress
                {
                    UserId = userid,
                    AddressId = address.AddressId
                };
                await _userAddressRepository.AddUserAddressAysnc(adduseraddress);
                transaction.Complete();
            }
        }
    }
}