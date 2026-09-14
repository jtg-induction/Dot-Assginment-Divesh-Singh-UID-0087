using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestaurantManagement;
using RestaurantManagement.Data;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Repository;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Effort;



namespace RestaurantManagement.tests.Repository
{
    [TestClass]
    /// <summary>
    /// Contains integration tests for <see cref="UserRepository"/> using a transient database.
    /// </summary>
    public class UserRepositoryTest
    {
        private ApplicationDbContext _context;
        private UserRepository _userrepo;


        [TestInitialize]
        public void setup()
        {
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

        /// <summary>Verifies that an unknown value is not reported as an existing phone number.</summary>
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
            var result = await _userrepo.PhoneNumberExistsAsync("new@example.com");
            Assert.IsFalse(result);
        }

        /// <summary>Verifies that a user can be added and retrieved by email.</summary>
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
            Assert.AreEqual(testUser, result);
        }



    }
}
