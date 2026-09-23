using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestaurantManagement.Constants;
using RestaurantManagement.Exceptions;
using RestaurantManagement.Models;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.repository;
using RestaurantManagement.Services;
using RestaurantManagement.Services.Exceptions;
using RestaurantManagement.Services.Interface;
using System;
using System.Threading.Tasks;

namespace RestaurantManagement.Tests.Services
{
    [TestClass]
    public class UserServiceTest
    {
        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IPasswordService> _passwordServiceMock;
        private UserService _userService;

        [TestInitialize]
        public void Setup()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordServiceMock = new Mock<IPasswordService>();

            _userService = new UserService(_userRepositoryMock.Object, _passwordServiceMock.Object);
        }

        [TestMethod]
        public async Task AdduserAsync_ValidDetails_CreatesAndReturnsUser()
        {
            // ARRANGE
            var request = new AddUserRequest
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "PlainPassword123",
                PhoneNumber = "1234567890",
                BirthDate = new DateTime(2000, 1, 1)
            };

            _userRepositoryMock.Setup(r => r.EmailExistsAsync(request.Email)).ReturnsAsync(false);
            _userRepositoryMock.Setup(r => r.PhoneNumberExistsAsync(request.PhoneNumber)).ReturnsAsync(false);
            _passwordServiceMock.Setup(s => s.HashPassword(request.Password)).Returns("HashedPasswordXYZ");
            _userRepositoryMock.Setup(r => r.AddUserAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // ACT
            var result = await _userService.AdduserAsync(request);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(request.Name, result.Name);
            Assert.AreEqual(request.Email, result.Email);
            Assert.AreEqual("HashedPasswordXYZ", result.Password);
            Assert.AreEqual(UserRole.Customer, result.Role);
        }

        [TestMethod]
        public async Task AdduserAsync_DuplicateEmail_ThrowsResourceException()
        {
            // ARRANGE
            var request = new AddUserRequest { Email = "duplicate@example.com" };
            _userRepositoryMock.Setup(r => r.EmailExistsAsync(request.Email)).ReturnsAsync(true);

            // ACT
            Exception e = null;
            try
            {
                await _userService.AdduserAsync(request);
            }
            // ASSERT
            catch (ResourceException ex)
            {
              
                e = ex;
            }
            //asert
            Assert.IsNotNull(e);
        }

        [TestMethod]
        public async Task AdduserAsync_DuplicatePhoneNumber_ThrowsResourceException()
        {
            // ARRANGE
            var request = new AddUserRequest { Email = "unique@example.com", PhoneNumber = "111111" };
            _userRepositoryMock.Setup(r => r.EmailExistsAsync(request.Email)).ReturnsAsync(false);
            _userRepositoryMock.Setup(r => r.PhoneNumberExistsAsync(request.PhoneNumber)).ReturnsAsync(true);

            // ACT
            Exception e = null;
            try
            {
                await _userService.AdduserAsync(request);
            }
            // ASSERT
            catch (ResourceException ex)
            {

                e = ex;
            }
            //asert
            Assert.IsNotNull(e);
        }

        [TestMethod]
        public async Task LoginUserAsync_ValidCredentials_ReturnsUser()
        {
            // ARRANGE
            var credential = new UserCredential { Email = "login@example.com", Password = "CorrectPassword" };
            var existingUser = new User { UserId = 10, Email = credential.Email, Password = "StoredHashedPassword" };

            _userRepositoryMock.Setup(r => r.GetUserAsync(credential.Email)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(r => r.IsActiveAsync(existingUser.UserId)).ReturnsAsync(true);
            _passwordServiceMock.Setup(s => s.VerifyPassword(credential.Password, existingUser.Password)).Returns(true);

            // ACT
            var result = await _userService.LoginUserAsync(credential);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(existingUser.UserId, result.UserId);
        }

        [TestMethod]
        public async Task LoginUserAsync_UserDoesNotExist_ThrowsUnauthenticatedException()
        {
            // ARRANGE
            var credential = new UserCredential { Email = "missing@example.com" };
            _userRepositoryMock.Setup(r => r.GetUserAsync(credential.Email)).ReturnsAsync((User)null);

            Exception e = null;
            try
            {
                await _userService.LoginUserAsync(credential);
            }
            // ASSERT
            catch (Exception ex)
            {

                e = ex;
            }
            //asert
            Assert.IsNotNull(e);
        }

        [TestMethod]
        public async Task LoginUserAsync_UserIsInactive_ThrowsUnauthenticatedException()
        {
            // ARRANGE
            var credential = new UserCredential { Email = "inactive@example.com" };
            var existingUser = new User { UserId = 20, Email = credential.Email };

            _userRepositoryMock.Setup(r => r.GetUserAsync(credential.Email)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(r => r.IsActiveAsync(existingUser.UserId)).ReturnsAsync(false);

            // ACT
            Exception e = null;
            try
            {
                await _userService.LoginUserAsync(credential);
            }
            // ASSERT
            catch (Exception ex)
            {

                e = ex;
            }
            //asert
            Assert.IsNotNull(e);

        }

        [TestMethod]
        public async Task LoginUserAsync_WrongPassword_ThrowsUnauthenticatedException()
        {
            // ARRANGE
            var credential = new UserCredential { Email = "user@example.com", Password = "WrongPassword" };
            var existingUser = new User { UserId = 30, Email = credential.Email, Password = "CorrectHashedPassword" };

            _userRepositoryMock.Setup(r => r.GetUserAsync(credential.Email)).ReturnsAsync(existingUser);
            _userRepositoryMock.Setup(r => r.IsActiveAsync(existingUser.UserId)).ReturnsAsync(true);
            _passwordServiceMock.Setup(s => s.VerifyPassword(credential.Password, existingUser.Password)).Returns(false);

            // ACT
            Exception e = null;
            try
            {
                await _userService.LoginUserAsync(credential);
            }
            // ASSERT
            catch (Exception ex)
            {

                e = ex;
            }
            //asert
            Assert.IsNotNull(e);
        }

        [TestMethod]
        public async Task GetUserAsync_ById_ReturnsExpectedUser()
        {
            // ARRANGE
            int targetId = 42;
            var expectedUser = new User { UserId = targetId, Name = "Divesh" };
            _userRepositoryMock.Setup(r => r.GetUserAsync(targetId)).ReturnsAsync(expectedUser);

            // ACT
            var result = await _userService.GetUserAsync(targetId);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(targetId, result.UserId);
            Assert.AreEqual("Divesh", result.Name);
        }

     
    }
}
