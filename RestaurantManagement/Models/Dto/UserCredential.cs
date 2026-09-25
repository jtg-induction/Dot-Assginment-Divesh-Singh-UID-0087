using RestaurantManagement.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models
{
    public class UserCredential
    {
        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        [Required(ErrorMessage = ValidationMessages.EmailRequired)]
        [EmailAddress(ErrorMessage = ValidationMessages.InvalidEmailFormat)]
        [RegularExpression(ValidationRules.EmailRegexPattern, ErrorMessage = ValidationMessages.InvalidEmailFormat)]
        public string Email { get; set; }

        [Required(ErrorMessage = ValidationMessages.PasswordRequired)]
        [StringLength(EntityConstants.MaxPasswordLength, MinimumLength = EntityConstants.MinPasswordLength, ErrorMessage = ValidationMessages.PasswordLength)]
        [RegularExpression(ValidationRules.PasswordRegexPattern, ErrorMessage = ValidationMessages.PasswordComplexity)]
        public string Password { get; set; }
    }
}