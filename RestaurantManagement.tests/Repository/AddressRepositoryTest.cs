using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Repository;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.tests.Repository
{
    [TestClass]
    public  class AddressRepositoryTest
    {
        private ApplicationDbContext _context;
        private AddressRepository _addressRepository;

        [TestInitialize]
        public void Setup()
        {
            DbConnection connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new ApplicationDbContext(connection);

            _addressRepository = new AddressRepository(_context);
        }
        [TestMethod]
        public async Task adduser()
        {
            // 1. Address 
            var address=new Address
            {
                AddressId = 1,
                Street = "123 Main Road",
                City = "Mumbai",
                State = "Maharashtra",
                PinCode = "400001",
                Country = "India",
                AddressType = AddressType.Home
            };
          _addressRepository.AddAddressAysnc(address);
            Assert.AreEqual(1, _context.Addresses.Count());

        }
        [TestMethod]
        public async Task getaddress()
        {
            var address = new Address
            {
                AddressId = 1,
                Street = "123 Main Road",
                City = "Mumbai",
                State = "Maharashtra",
                PinCode = "400001",
                Country = "India",
                AddressType = AddressType.Home
            };
            _addressRepository.AddAddressAysnc(address);
            String respomse=await _addressRepository.GetAddress(1);
            Assert.IsNotEmpty(respomse);
        }
    }
}
