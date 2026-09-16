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
        [EmailAddress(ErrorMessage =ValidationMessages.InValidEmail)]
        [StringLength(100)]
        public string Email { get; set; }
        [Required(ErrorMessage =ValidationMessages.PasswordRequired)]
        [StringLength(EntityConstants.MaxPasswordLength, MinimumLength = EntityConstants.MinPasswordLength)]
        [RegularExpression(ValidationRules.PasswordRegexPattern)]
        public string Password { get; set; }
    }
}