using System;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Effort;
using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Repository;

namespace RestaurantManagement.tests.Repository
{
    [TestClass]
    /// <summary>
    /// Contains integration tests for <see cref="UserRepository"/> using a transient in-memory database provider.
    /// </summary>
    public class UserRepositoryTest
    {
        private ApplicationDbContext _context;
        private UserRepository _userrepo;

        [TestInitialize]
        public void setup()
        {
            // Effort creates a lightweight, schema-compliant in-memory relational instance for EF6
            DbConnection connection = Effort.DbConnectionFactory.CreateTransient();
            _context = new ApplicationDbContext(connection);
            _userrepo = new UserRepository(_context);
        }
        /// <summary>Verifies that an existing email is detected.</summary>
        [TestMethod]
        public async Task CheckEmailIsPresent()
        {
            var testUser = new User
            {
                Email = "divesh@gmail.com",
                Name = "DIVESH",
                Password = "89hubh",
                PhoneNumber = "0i09896",
                BirthDate = DateTime.Parse("2002-01-01 00:00:00"),
                Balance = 1000,
                Role = UserRole.Customer
            };

            _context.Users.Add(testUser);
            await _context.SaveChangesAsync();

            var result = await _userrepo.EmailExistsAsync("divesh@gmail.com");
            Assert.IsTrue(result);
        }

        /// <summary>Verifies that an unknown email is not reported as existing.</summary>
        [TestMethod]
        public async Task CheckEmailsNotPresent()
        {
            var testUser = new User
            {
                Email = "divesh@gmail.com",
                Name = "DIVESH",
                Password = "89hubh",
                PhoneNumber = "0309896",
                BirthDate = DateTime.Parse("2002-01-01 00:00:00"),
                Balance = 1000,
                Role = UserRole.Customer
            };

            _context.Users.Add(testUser);
            await _context.SaveChangesAsync();

            var result = await _userrepo.EmailExistsAsync("new@example.com");
            Assert.IsFalse(result);
        }

        /// <summary>Verifies that an existing phone number is detected.</summary>
        [TestMethod]
        public async Task CheckPhoneNumberIsPresent()
        {
            var testUser = new User
            {
                Email = "divesh@gmail.com",
                Name = "DIVESH",
                Password = "89hubh",
                PhoneNumber = "0309896",
                BirthDate = DateTime.Parse("2002-01-01 00:00:00"),
                Balance = 1000,
                Role = UserRole.Customer
            };

            _context.Users.Add(testUser);
            await _context.SaveChangesAsync();

            var result = await _userrepo.PhoneNumberExistsAsync("0309896");
            Assert.IsTrue(result);
        }

        /// <summary>Verifies that an unknown phone number is not detected.</summary>
        [TestMethod]
        public async Task CheckPhoneNumberIsNotPresent()
        {
            var testUser = new User
            {
                Email = "divesh@gmail.com",
                Name = "DIVESH",
                Password = "89hubh",
                PhoneNumber = "0309896",
                BirthDate = DateTime.Parse("2002-01-01 00:00:00"),
                Balance = 1000,
                Role = UserRole.Customer
            };

            _context.Users.Add(testUser);
            await _context.SaveChangesAsync();

            var result = await _userrepo.PhoneNumberExistsAsync("030996");
            Assert.IsFalse(result);
        }

        /// <summary>Verifies that a user can be added and successfully retrieved by email string.</summary>
        [TestMethod]
        public async Task Adduser()
        {
            var testUser = new User
            {
                Email = "divesh@gmail.com",
                Name = "DIVESH",
                Password = "89hubh",
                PhoneNumber = "0309896",
                BirthDate = DateTime.Parse("2002-01-01 00:00:00"),
                Balance = 1000,
                Role = UserRole.Customer
            };

            await _userrepo.AddUserAsync(testUser);
            var result = await _userrepo.GetUserAsync("divesh@gmail.com");
            Assert.IsNotNull(result);
            Assert.AreEqual(testUser.Email, result.Email);
            Assert.AreEqual(testUser.Name, result.Name);
        }

        /// <summary>Verifies that a user can be retrieved using their Primary Key ID number.</summary>
        [TestMethod]
        public async Task GetUserById_ExistingId_ReturnsUser()
        {
            var testUser = new User
            {
                Email = "findbyid@gmail.com",
                Name = "FindMe",
                Password = "securePassword",
                PhoneNumber = "9999999",
                BirthDate = DateTime.Parse("1995-05-05"),
                Balance = 500,
                Role = UserRole.Customer
            };

            _context.Users.Add(testUser);
            await _context.SaveChangesAsync();

            var result = await _userrepo.GetUserAsync(testUser.UserId);

            Assert.IsNotNull(result);
            Assert.AreEqual(testUser.UserId, result.UserId);
        }
    }
}
