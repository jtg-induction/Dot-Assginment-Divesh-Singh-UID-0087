using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestaurantManagement.Services;
using RestaurantManagement.Services.Interface;
using System;

namespace RestaurantManagement.Tests
{
    [TestClass]
    public class PasswordServiceTests
    {
        private IPasswordService _passwordService;

        [TestInitialize]
        public void SetUp()
        {
            _passwordService = new PasswordService();
        }

        [TestMethod]
        public void HashPassword_WhenGivenValidPassword_ShouldReturnHashedString()
        {
            // ARRANGE
            string plainTextPassword = "Lhubyyhb@1";

            // ACT
            string hashedPassword = _passwordService.HashPassword(plainTextPassword);

            // ASSERT
            Assert.IsFalse(string.IsNullOrWhiteSpace(hashedPassword));
            Assert.AreNotEqual(plainTextPassword, hashedPassword);
        }

        [TestMethod]
        public void VerifyPassword_WhenPasswordMatchesHash_ShouldReturnTrue()
        {
            // ARRANGE
            string plainTextPassword = "Lhubyyhb@1";
            string hashedPassword = _passwordService.HashPassword(plainTextPassword);

            // ACT
            bool isValid = _passwordService.VerifyPassword(plainTextPassword, hashedPassword);

            // ASSERT
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void VerifyPassword_WhenPasswordDoesNotMatchHash_ShouldReturnFalse()
        {
            // ARRANGE
            string correctPassword = "Lhubyyhb@1";
            string wrongPassword = "WrongPassword123";
            string hashedPassword = _passwordService.HashPassword(correctPassword);

            // ACT
            bool isValid = _passwordService.VerifyPassword(wrongPassword, hashedPassword);

            // ASSERT
            Assert.IsFalse(isValid);
        }
    }
}
