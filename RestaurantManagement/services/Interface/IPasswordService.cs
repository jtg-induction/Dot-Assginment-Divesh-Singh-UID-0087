namespace RestaurantManagement.Services.Interface
{
    /// <summary>
    /// Provides password hashing and verification operations.
    /// </summary>
    public interface IPasswordService
    {
        /// <summary>
        /// Hashes a password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The password hash.</returns>
        string HashPassword(string password);

        /// <summary>
        /// Verifies a password against a hash.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="hashedPassword">The hash to verify against.</param>
        /// <returns><see langword="true"/> when the password matches; otherwise, <see langword="false"/>.</returns>
        bool VerifyPassword(string password, string hashedPassword);
    }
}