using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestaurantManagement.Constants;
using RestaurantManagement.Exceptions;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Models.Response;
using RestaurantManagement.repository;
using RestaurantManagement.Repository;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.WebPages;

namespace RestaurantManagement.Services
{


    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IRestaurantOwnerRepository _restaurantOwnerRepository;
        public RestaurantService(IRestaurantRepository restaurantRepository, IUserRepository userRepository, IAddressRepository addressRepository, IRestaurantOwnerRepository restaurantOwnerRepository)
        {
            _restaurantRepository = restaurantRepository;
            _userRepository = userRepository;
            _addressRepository = addressRepository;
            _restaurantOwnerRepository = restaurantOwnerRepository;
        }
        public async Task<List<ActiveRestaurantResponse>> GetRestaurantsAsync()
        {
            var activeRestaurants = await _restaurantRepository.GetRestaurantsAsync();
            var data1 = new List<ActiveRestaurantResponse>();
            foreach (Restaurant restaurant in activeRestaurants)
            {
                var data = await _addressRepository.GetAddress(restaurant.AddressId);
                string address = $"{data.Street},{data.City},{data.State},{data.Country},{data.PinCode},{data.AddressType}";
                data1.Add(new ActiveRestaurantResponse
                {
                    RestaurantId = restaurant.RestaurantId,
                    Name = restaurant.Name,
                    Address = address,
                    Email = restaurant.Email,
                    PhoneNumber = restaurant.PhoneNumber

                });
            }
            return data1;
        }
        public async Task AddRestaurant(AddRestaurantRequest addRestaurant)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                if (await _restaurantRepository.EmailExixts(addRestaurant.Email))
                {
                    throw new ResourceException(ValidationMessages.DuplicateEmail);
                }
                if (await _restaurantRepository.PhoneNumberExixts(addRestaurant.PhoneNumber))
                {
                    throw new ResourceException(ValidationMessages.DuplicatePhone);
                }
                foreach (string email in addRestaurant.UserEmail)
                {
                    if (!await _userRepository.EmailExistsAsync(email))
                    {
                        throw new ResourceException($"{email} Not Found!!");
                    }
                }
                var address = new Address
                {
                    Street=addRestaurant.Street,
                    City=addRestaurant.City,
                    State=addRestaurant.State,
                    PinCode=addRestaurant.Pincode,
                    Country=addRestaurant.Pincode,
                    AddressType=addRestaurant.AddressType
                };
               await  _addressRepository.AddAddressAysnc(address);
                var restaurant = new Restaurant
                {
                    Name = addRestaurant.Name,
                    AddressId = address.AddressId,
                    Email = addRestaurant.Email,
                    PhoneNumber = addRestaurant.PhoneNumber
                };
                await _restaurantRepository.AddRestaurant(restaurant);

                List<RestaurantOwner> restaurantOwners = new List<RestaurantOwner>();
                foreach (string email in addRestaurant.UserEmail)
                {
                    restaurantOwners.Add(new RestaurantOwner
                    {
                        RestaurantId = restaurant.RestaurantId,
                        UserId = (await _userRepository.GetUserAsync(email)).UserId

                    });
                }
                await _restaurantOwnerRepository.AddRestaurantOwner(restaurantOwners);
                await _userRepository.ChangeRoleToOwner(addRestaurant.UserEmail);
                transaction.Complete();
            }
        }
        public async Task AddRestaurantowner(AddRestaurantOwnerRequest addRestaurantOwnerRequest)
        {
            var restaurant = await _restaurantRepository.RestaurantIsActive(addRestaurantOwnerRequest.RestaurantEmail);
            if (restaurant==null)
            {
                throw new ResourceException(ValidationMessages.RestaurantNotFound);
            }
            foreach (string email in addRestaurantOwnerRequest.UserEmail)
            {
                if (!await _userRepository.EmailExistsAsync(email))
                {
                    throw new ResourceException($"{email} Not Found!!");
                }
            }
            List<RestaurantOwner> restaurantOwners = new List<RestaurantOwner>();
            foreach (string email in addRestaurantOwnerRequest.UserEmail)
            {
                restaurantOwners.Add(new RestaurantOwner
                {
                    RestaurantId = restaurant.RestaurantId,
                    UserId = (await _userRepository.GetUserAsync(email)).UserId

                });
            }
            await _restaurantOwnerRepository.AddRestaurantOwner(restaurantOwners);
            await _userRepository.ChangeRoleToOwner(addRestaurantOwnerRequest.UserEmail);

        }
      
    }
}