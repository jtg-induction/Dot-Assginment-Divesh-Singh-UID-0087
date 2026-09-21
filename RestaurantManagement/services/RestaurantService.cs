using Microsoft.IdentityModel.Tokens;
using RestaurantManagement.Constants;
using RestaurantManagement.Exceptions;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
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
        public async Task<List<Restaurant>> GetRestaurantsAsync()
        {
            return await _restaurantRepository.GetRestaurantsAsync();
        }
        public async Task<string> GetRestaurantName(int id)
        {
            return await _restaurantRepository.GetRestaurantName(id);
        }
        public async Task AddRestaurant(AddRestaurantRequest addRestaurant)
        {
            using (TransactionScope transaction =new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)){
                if (await _restaurantRepository.EmailExixts(addRestaurant.Email))
                {
                    throw new ResourceException(ValidationMessages.DuplicateEmail);
                }
                if (await _restaurantRepository.PhoneNumberExixts(addRestaurant.PhoneNumber))
                {
                    throw new ResourceException(ValidationMessages.DuplicatePhone);
                }
                if (!await _userRepository.UserExists(addRestaurant.UserId))
                {
                    throw new ResourceException(ValidationMessages.UserNotFound);
                }
                if ((await _addressRepository.GetAddress(addRestaurant.AddressId)).IsEmpty())
                {
                    throw new ResourceException(ValidationMessages.AddressNotFound);
                }

                var restaurant = new Restaurant
                {
                    Name = addRestaurant.Name,
                    AddressId = addRestaurant.AddressId,
                    Email = addRestaurant.Email,
                    PhoneNumber = addRestaurant.PhoneNumber
                };
                await _restaurantRepository.AddRestaurant(restaurant);

                List<RestaurantOwner> restaurantOwners = new List<RestaurantOwner>();
                foreach (int i in addRestaurant.UserId)
                {
                    restaurantOwners.Add(new RestaurantOwner
                    {
                        RestaurantId = restaurant.RestaurantId,
                        UserId = i

                    });
                }
                await _restaurantOwnerRepository.AddRestaurantOwner(restaurantOwners);
                await _userRepository.ChangeRoleToOwner(addRestaurant.UserId);
                transaction.Complete();
            }
        }
        public async Task AddRestaurantowner(AddRestaurantOwnerRequest addRestaurantOwnerRequest)
        {
            if((await _restaurantRepository.GetRestaurantName(addRestaurantOwnerRequest.RestaurantId)).IsEmpty())
            {
                throw new ResourceException(ValidationMessages.RestaurantNotFound);
            }
            if (!await _userRepository.UserExists(addRestaurantOwnerRequest.UserId))
            {
                throw new ResourceException(ValidationMessages.UserNotFound);
            }
            List<RestaurantOwner> restaurantOwners = new List<RestaurantOwner>();
            foreach (int i in addRestaurantOwnerRequest.UserId)
            {
                restaurantOwners.Add(new RestaurantOwner
                {
                    RestaurantId = addRestaurantOwnerRequest.RestaurantId,
                    UserId = i

                });
            }
            await _restaurantOwnerRepository.AddRestaurantOwner(restaurantOwners);
            await _userRepository.ChangeRoleToOwner(addRestaurantOwnerRequest.UserId);

        }
    }
}