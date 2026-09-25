using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Repository.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantManagement.Repository
{
    public class UserAddressRepository : IUserAddressRepository
    {
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used to access user records.</param>
        public UserAddressRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public async Task<List<Address>> GetUserAddress(int id)
        {
             return await _db.UserAddresses.Where(e => e.UserId == id).Select(e => e.Address).ToListAsync();           
        }
        public async Task AddUserAddressAysnc(UserAddress userAddress)
        {
             _db.UserAddresses.Add(userAddress);
           await _db.SaveChangesAsync();
        }


    }
}