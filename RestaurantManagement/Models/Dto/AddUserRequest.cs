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
                [Required(ErrorMessage = ValidationMessages.NameRequired)]
                public string Name { get; set; }

                /// <summary>
                /// Gets or sets the user's password.
                /// </summary>
                [Required(ErrorMessage = ValidationMessages.PasswordRequired)]
                [StringLength(EntityConstants.MaxPasswordLength, MinimumLength = EntityConstants.MinPasswordLength, ErrorMessage = ValidationMessages.PasswordLength)]
                [RegularExpression(ValidationRules.PasswordRegexPattern, ErrorMessage = ValidationMessages.PasswordComplexity)]
                public string Password { get; set; }

                /// <summary>
                /// Gets or sets the user's email address.
                /// </summary>
                [Required(ErrorMessage = ValidationMessages.EmailRequired)]
                [EmailAddress(ErrorMessage = ValidationMessages.InvalidEmailFormat)]
                public string Email { get; set; }

                /// <summary>
                /// Gets or sets the user's birth date.
                /// </summary>
                [Required(ErrorMessage = ValidationMessages.BirthDateRequired)]
                public DateTime BirthDate { get; set; }

                /// <summary>
                /// Gets or sets the user's phone number.
                /// </summary>
                [Required(ErrorMessage = ValidationMessages.PhoneRequired)]
                [RegularExpression(ValidationRules.PhoneRegexPattern, ErrorMessage = ValidationMessages.InvalidPhoneFormat)]
                public string PhoneNumber { get; set; }
        }
}
