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
            await _userService.AdduserAsync(request);

            // ASSERT
           
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
        [TestMethod]
        public void activateacount()
        {
            UserCredential user = new UserCredential
            {
                Email = "ndkjbvid@abc.com",
                Password = "nfjdvbid"
            };
            var incoming = new User
            {
                IsActive = false
            };
            _userRepositoryMock.Setup(e => e.GetUserAsync(user.Email)).ReturnsAsync(incoming);
            _userRepositoryMock.Setup(e => e.Activate(incoming));
           var response=_userService.ActivateAccount(user);
            Assert.IsNotNull(response);

        }
        [TestMethod]
        public void deactivateacount()
        {
            UserCredential user = new UserCredential
            {
                Email = "ndkjbvid@abc.com",
                Password = "nfjdvbid"
            };
            var incoming = new User
            {
                UserId=1,
                IsActive = false
            };
            _userRepositoryMock.Setup(e => e.GetUserAsync(incoming.UserId)).ReturnsAsync(incoming);
            _userRepositoryMock.Setup(e => e.Deactivate(incoming));
            var response = _userService.DeactivateAccount(incoming.UserId);
            Assert.IsNotNull(response);
            _userRepositoryMock.Setup(e => e.GetUserAsync(incoming.UserId)).ReturnsAsync((User)null);
            response = _userService.DeactivateAccount(incoming.UserId);
            Assert.IsNotNull(response);

        }
        [TestMethod]
        public void updateacount()
        {
            User user = new User
            {
                Email = "ndkjbvid@abc.com",
                PhoneNumber = "8786765544"
            };
            UpdateAccountDto update = new UpdateAccountDto
            {
                Email = "abc@abc.com"
            };
            _userRepositoryMock.Setup(e => e.EmailExistsAsync(user.Email)).ReturnsAsync(false);
            _userRepositoryMock.Setup(e => e.PhoneNumberExistsAsync(user.PhoneNumber)).ReturnsAsync(false);
            _userRepositoryMock.Setup(e => e.UpdateAccount(user, update));
            Exception e = null;
            try
            {
                _userService.UpdateAccount(user, update);
            } catch (Exception ex)
            {
                ex = e;
            }
            Assert.IsNull(e);
        }
        [TestMethod]
        public async Task getuserifactiveAsync()
        {
            User user = new User
            {
                UserId=1,
                Email = "ndkjbvid@abc.com",
                PhoneNumber = "8786765544"
            };
            _userRepositoryMock.Setup(e => e.IsActiveAsync(user.UserId)).ReturnsAsync(true);
            _userRepositoryMock.Setup(e => e.GetUserAsync(user.UserId)).ReturnsAsync(user);
            var response= await _userService.GetUserIfActive(user.UserId);
            Assert.IsNotNull(response);

        }
        [TestMethod]
        public async Task UpdateAccount_WhenFieldsAreEmpty_FallsBackToUserPropertiesAndSucceeds()
        {
            // Arrange
            var existingUser = new User
            {
                UserId = 1,
                Name = "John Doe",
                Email = "john@example.com",
                PhoneNumber = "5551234",
                BirthDate = new DateTime(1990, 1, 1)
            };

            // DTO has empty strings and default DateTime
            var updateDto = new UpdateAccountDto
            {
                Name = "", // Assuming .IsEmpty() handles empty strings
                Email = "",
                PhoneNumber = "",
                BirthDate = DateTime.MinValue
            };

            _userRepositoryMock.Setup(repo => repo.IsActiveAsync(existingUser.UserId)).ReturnsAsync(true);
            _userRepositoryMock.Setup(repo => repo.EmailExistsAsync(updateDto.Email)).ReturnsAsync(false);
            _userRepositoryMock.Setup(repo => repo.PhoneNumberExistsAsync(updateDto.PhoneNumber)).ReturnsAsync(false);

            // Act
            await _userService.UpdateAccount(existingUser, updateDto);

            // Assert
            // Verify values were successfully copied from 'existingUser' to 'updateDto' before saving
            Assert.AreEqual(existingUser.Name, updateDto.Name);
        }
        }
}
