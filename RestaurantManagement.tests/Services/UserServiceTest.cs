using Moq;
using RestaurantManagement.Constants;
using BCrypt.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
//using RestaurantManagement.Common;
using RestaurantManagement.Controllers;
using RestaurantManagement.Models;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.repository;
using RestaurantManagement.Services;
using RestaurantManagement.Services.Interface;
using System;
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
        public void Setup()
        {
            _mockrepo = new Mock<IUserRepository>();
            _mockpass = new Mock<IPasswordService>();
            _userser = new UserService(_mockrepo.Object, _mockpass.Object);

        }

        #region Adduser Method Tests

        /// <summary>
        /// Verifies that a user with unique contact details is added successfully.
        /// </summary>
        [TestMethod]
        public async Task ValidDto()
        {
            // Arrange
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
            // Arrange
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

        #endregion

        #region CheckUser Method Tests

        /// <summary>
        /// Verifies that a valid credential mapping matches the hash securely and logs in the profile context.
        /// </summary>
        [TestMethod]
        public async Task CheckUser_CorrectCredentials_ReturnsMatchedUser()
        {
            // Arrange
            string inputPlaintext = "123234@aA";
            string validBCryptString = BCrypt.Net.BCrypt.HashPassword(inputPlaintext);

            var trackingCredentials = new UserCredential { Email = "divesh@gmail.com", Password = inputPlaintext };
            var foundDbUser = new User { UserId = 1, Email = "divesh@gmail.com", Password = validBCryptString };

            _mockrepo.Setup(r => r.GetUserAsync(trackingCredentials.Email)).ReturnsAsync(foundDbUser);

            // ACT
            var evaluatedUser = await _userser.CheckUserAsync(trackingCredentials);

            // Assert
            Assert.IsNotNull(evaluatedUser);
            Assert.AreEqual(foundDbUser.UserId, evaluatedUser.UserId);
        }

        /// <summary>
        /// Verifies that bad credentials fail validation routing gates and discard user assignment structures.
        /// </summary>
        [TestMethod]
        public async Task CheckUser_IncorrectPassword_ReturnsNull()
        {
            // Arrange
            string mismatchedPlaintext = "incorrectPassword";
            string realBCryptHash = BCrypt.Net.BCrypt.HashPassword("correctPassword");

            var trackingCredentials = new UserCredential { Email = "divesh@gmail.com", Password = mismatchedPlaintext };
            var foundDbUser = new User { UserId = 1, Email = "divesh@gmail.com", Password = realBCryptHash };

            _mockrepo.Setup(r => r.GetUserAsync(trackingCredentials.Email)).ReturnsAsync(foundDbUser);

            // ACT
            var evaluatedUser = await _userser.CheckUserAsync(trackingCredentials);

            // Assert
            Assert.IsNull(evaluatedUser);
        }

        /// <summary>
        /// Verifies searching an unregistered email addresses stops computation and responds with a null handle.
        /// </summary>
        [TestMethod]
        public async Task CheckUser_UnregisteredEmail_ReturnsNull()
        {
            // Arrange
            var trackingCredentials = new UserCredential { Email = "nonexistent@gmail.com", Password = "any" };
            _mockrepo.Setup(r => r.GetUserAsync(trackingCredentials.Email)).ReturnsAsync((User)null);

            // ACT
            var evaluatedUser = await _userser.CheckUserAsync(trackingCredentials);

            // Assert
            Assert.IsNull(evaluatedUser);
        }

        #endregion

        #region GetUser Method Tests

        /// <summary>
        /// Verifies that passing an identity integer retrieves the matching entity footprint from the database.
        /// </summary>
        [TestMethod]
        public async Task GetUser_ValidIdentifier_ReturnsMatchingRecord()
        {
            // Arrange
            int queryTargetId = 15;
            var baselineUser = new User { UserId = queryTargetId, Name = "Divesh Profile" };

            _mockrepo.Setup(r => r.GetUserAsync(queryTargetId)).ReturnsAsync(baselineUser);

            // ACT
            var matchedUserResult = await _userser.GetUserAsync(queryTargetId);

            // Assert
            Assert.IsNotNull(matchedUserResult);
            Assert.AreEqual(queryTargetId, matchedUserResult.UserId);
            _mockrepo.Verify(r => r.GetUserAsync(queryTargetId), Times.Once);
        }

        #endregion
    }
}
