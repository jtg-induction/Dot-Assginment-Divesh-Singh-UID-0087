using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantManagement.Repository
{
    public class AddressRepository :IAddressRepository
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
        //public async Task<List<Address>> GetAddressAsync(int id)
        //{
        //    return await _db.
        //}
        public async Task AddAddressAysnc(Address address)
        {
            _db.Addresses.Add(address);
            await _db.SaveChangesAsync();
        }
        public async Task<string> GetAddress(int id)
        {
            var data =await  _db.Addresses.FindAsync(id);
            if (data == null)
            {
                return string.Empty;
            }
            string address = $"{data.Street},{data.City},{data.State},{data.Country},{data.PinCode},{data.AddressType}";
            return address;
        }

    }
}