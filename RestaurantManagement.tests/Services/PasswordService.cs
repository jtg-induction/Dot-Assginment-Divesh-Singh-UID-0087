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
            // Initialize the actual service implementation (do not mock this)
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
            Assert.IsFalse(string.IsNullOrWhiteSpace(hashedPassword), "The password hash should not be empty.");
            Assert.AreNotEqual(plainTextPassword, hashedPassword, "The hashed password must not match the plain text password.");
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
            Assert.IsTrue(isValid, "VerifyPassword should return true when the plain text matches the hash.");
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
            Assert.IsFalse(isValid, "VerifyPassword should return false when given an incorrect password.");
        }
    }
}
