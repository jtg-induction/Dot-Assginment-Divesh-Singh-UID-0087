using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantManagement.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used to access user records.</param>
        public AddressRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public async Task AddAddressAsync(Address address)
        {
            _db.Addresses.Add(address);
            await _db.SaveChangesAsync();
        }
        public async Task<Address> GetAddressAsync(int id, int userid)
        {
            var data = _db.UserAddresses.Where(e => e.UserId == userid && e.AddressId == id).Select(e => e.Address);
            return await data.FirstOrDefaultAsync();
        }
        public async Task<Address> GetAddressAsync(int id)
        {
            return await _db.Addresses.FindAsync(id);
        }

    }
}