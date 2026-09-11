using Moq;
using RestaurantManagement.Constants;
using RestaurantManagement.Controllers;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.repository;
using RestaurantManagement.Services;
using RestaurantManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.tests.Services
{
    [TestClass]
    /// <summary>
    /// Contains unit tests for <see cref="UserService"/> user creation behavior.
    /// </summary>
    public class UserServiceTest
    {
        private Mock<IUserRepository> _mockrepo;
        private Mock<IPasswordService> _mockpass;
        private UserService _userser;
        [TestInitialize]
        public void setup()
        {
            _mockrepo = new Mock<IUserRepository>();
            _mockpass = new Mock<IPasswordService>();
            _userser = new UserService(_mockrepo.Object, _mockpass.Object);

        }

        /// <summary>
        /// Verifies that a user with unique contact details is added successfully.
        /// </summary>
        [TestMethod]
        public async Task ValidDto()
        {
            //Arrange
            var testuser = new AddUserRequest()
            {
                Name = "aabb",
                Password = "Lhubyyhb@1",
                Email = "jnjnu@hh.com",
                PhoneNumber = "7680987879",
                BirthDate = DateTime.Parse("2000-01-01 00:00:00")
            };
            _mockrepo.Setup(e => e.EmailExistsAsync(testuser.Email)).ReturnsAsync(false);
            _mockrepo.Setup(e => e.PhoneNumberExistsAsync(testuser.PhoneNumber)).ReturnsAsync(false);
            _mockrepo.Setup(e => e.AddUserAsync(It.IsAny<User>()));
            //ACT
            Exception thrownException = null;
            try
            {
                await _userser.AdduserAsync(testuser);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }

            // ASSERT
            // 1. Verify no exceptions (like ResourceException) were thrown
            Assert.IsNull(thrownException);

            //Assert.Fail(res);

        }

        /// <summary>
        /// Verifies that adding a user with an existing email is rejected.
        /// </summary>
        [TestMethod]
        public async Task DuplicateEmail()
        {
            //Arrange
            var testuser = new AddUserRequest()
            {
                Name = "aabb",
                Email = "jnjnu@hh.com",
                PhoneNumber = "768099",
                BirthDate = DateTime.Parse("2000-01-01 00:00:00")
            };
            _mockrepo.Setup(e => e.EmailExistsAsync(testuser.Email)).ReturnsAsync(true);
            _mockrepo.Setup(e => e.PhoneNumberExistsAsync(testuser.PhoneNumber)).ReturnsAsync(false);
            _mockrepo.Setup(e => e.AddUserAsync(It.IsAny<User>()));
            //ACT
            Exception thrownException = null;
            try
            {
                await _userser.AdduserAsync(testuser);
            }
            catch (Exception ex)
            {
                thrownException = ex;
            }

            // ASSERT
            // 1. Verify no exceptions (like ResourceException) were thrown
            Assert.IsNotNull(thrownException);

        }
    }
}
