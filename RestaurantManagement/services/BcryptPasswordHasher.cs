using RestaurantManagement.Services.Interface;

namespace RestaurantManagement.Services
{
    /// <summary>
    /// Provides password hashing and verification using BCrypt.
    /// </summary>
    public class BcryptPasswordHasher : IPasswordHasher
    {
        private const int WorkFactor = 12;

        /// <summary>
        /// Hashes a plain-text password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The BCrypt password hash.</returns>
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        /// <summary>
        /// Verifies a password against a BCrypt hash.
        /// </summary>
        /// <param name="password">The plain-text password to verify.</param>
        /// <param name="hashedPassword">The BCrypt hash to verify against.</param>
        /// <returns><see langword="true"/> if the password matches; otherwise, <see langword="false"/>.</returns>
        public bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
