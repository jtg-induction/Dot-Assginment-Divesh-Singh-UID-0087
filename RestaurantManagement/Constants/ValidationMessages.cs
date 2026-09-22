namespace RestaurantManagement.Constants
{
    /// <summary>
    /// Contains validation messages used by the application.
    /// </summary>
    public class ValidationMessages
    {
        public const string DuplicateEmail = "This email address is already registered.";
        public const string DuplicatePhone = "This phone number is already registered.";
        public const string UserCreated = "User registered successfully";
        public const string PhoneRequired = "Phone number is required.";
        public const string InvalidPhoneFormat = "Invalid phone number format. It must contain only 10 digits.";
        public const string InternalServerError = "An unexpected error occurred on the server. Please try again later.";
        public const string NameRequired = "Name is required.";
        public const string PasswordRequired = "Password is required.";
        public const string PasswordLength = "Password does not meet the length requirements.";
        public const string PasswordComplexity = "Password must contain uppercase, lowercase, numbers, and special characters.";
        public const string EmailRequired = "Email address is required.";
        public const string InvalidEmailFormat = "Please provide a valid email address format.";
        public const string BirthDateRequired = "Birth date is required.";
        public const string ValidationError = "One or more validation errors occurred.";
        public const string InvalidRequest = "Invalid request format.";
        public const string Field = "field";
        public const string LoginSuccess = "Login successful.";
        public const string UpdateSuccess = "Updated successful.";
        public const string Revoked = "Invalid Token";
        public const string UserNotFound = "User Not Found!!";
        public const string InvalidDetail = "User Credential Invalid!!";
        public const string LogoutSucess = "Logged out successfully.";
        public const string RefreshSuccess = "Refresh successfully.";
        public const string DeactivateSuccess = "Deactivate successfully.";
        public const string ActivateSuccess = "Activate successfully.";
    }
}