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
    public  class UserAddressRepositoryTest
    {
        private ApplicationDbContext _context;
        private UserAddressRepository _userAddress;

        [TestInitialize]
        public void Setup()
        {
            DbConnection connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new ApplicationDbContext(connection);

            _userAddress = new UserAddressRepository(_context);
        }
        [TestMethod]
        public async Task Getuseraddress()
        {
            // 1. Address 
            _context.Addresses.Add(new Address
            {
                AddressId = 1,
                Street = "123 Main Road",
                City = "Mumbai",
                State = "Maharashtra",
                PinCode = "400001",
                Country = "India",
                AddressType = AddressType.Home
            });
            _context.Users.Add(new User
            {
                UserId = 1,
                Name = "John Doe",
                Password = "SecurePassword123!",
                Email = "testuser@example.com",
                BirthDate = new DateTime(1995, 5, 15),
                PhoneNumber = "123-456-7890",
                Role = UserRole.Customer,
                Balance = 1000m,
                IsActive = true,
                BalanceUpdatedAt = DateTime.UtcNow
            });

            UserAddress address = new UserAddress{
                UserAddressId=1,
                UserId=1,AddressId=1
                };
           await  _userAddress.AddUserAddressAysnc(address);
            var result = await _userAddress.GetUserAddress(1);
            //Assert.Fail(result.Count());
            Assert.AreEqual(1, result.Count());
        }

    }
}
