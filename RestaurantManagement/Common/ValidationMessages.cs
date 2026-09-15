namespace RestaurantManagement.Common
{
    /// <summary>
    /// Contains validation messages used by the application.
    /// </summary>
    public class ValidationMessages
    {
        /// <summary>
        /// Indicates that the email address and phone number already exist.
        /// </summary>
        public const string DuplicateEmailAndPhone = "Same email and phone number";

        /// <summary>
        /// Indicates that an operation completed successfully.
        /// </summary>
        public const string Success = "succes";
        public const string Revoked = "Refresh Token is already revoked";
        public const string InValidToken = "Token Invalid";
    }
}