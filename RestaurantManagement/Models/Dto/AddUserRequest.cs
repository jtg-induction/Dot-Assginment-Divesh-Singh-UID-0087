using System;
using System.ComponentModel.DataAnnotations;
using RestaurantManagement.Constants;

namespace RestaurantManagement.Models.Dto
{
    /// <summary>
    /// Represents the information required to create a user.
    /// </summary>
    public class AddUserRequest
    {
        /// <summary>
        /// Gets or sets the user's name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the user's password.
        /// </summary>
        [Required]
        [StringLength(EntityConstants.MaxPasswordLength, MinimumLength =EntityConstants.MinPasswordLength)]
        [RegularExpression(ValidationRules.PasswordRegexPattern)]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's birth date.
        /// </summary>
        [Required]
        public  DateTime BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the user's phone number.
        /// </summary>
        [Required]
        [RegularExpression(ValidationRules.PhoneRegexPattern, ErrorMessage = ValidationMessages.InvalidPhoneFormat)]
        public string PhoneNumber { get; set; }

    }
}