namespace RestaurantManagement.Constants
{
        /// <summary>
        /// Contains validation messages used by the application.
        /// </summary>
        public class ValidationMessages
        {
                public const string DuplicateEmail = "This email address is already registered.";
                public const string DuplicatePhone = "This phone number is already registered.";
                public const string Success = "Success";
                public const string PhoneRequired = "Phone number is required.";
                public const string InvalidPhoneFormat = "Invalid phone number format. It must contain only 10 digits.";
                public const string InternalServerError = "An unexpected error occurred on the server. Please try again later.";
                public const string Revoked = "Invalid Token";
                public const string NotFound = "Not Found";
                public const string InValidEmail = "Invalid email format.";
                public const string PasswordRequired = "Password is required.";
        }
}