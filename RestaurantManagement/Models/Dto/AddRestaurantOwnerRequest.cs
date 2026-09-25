using RestaurantManagement.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Dto
{
    public class AddRestaurantOwnerRequest
    {
        [Required(ErrorMessage ="RestaurantEmail Required!!")]
        [EmailAddress(ErrorMessage = ValidationMessages.InvalidEmailFormat)]
        [RegularExpression(ValidationRules.PasswordRegexPattern, ErrorMessage = ValidationMessages.PasswordComplexity)]
        public string RestaurantEmail { get; set; }

        [Required(ErrorMessage = ValidationMessages.UserEmailRequired)]
        [EmailAddress(ErrorMessage = ValidationMessages.InvalidEmailFormat)]
        [RegularExpression(ValidationRules.PasswordRegexPattern, ErrorMessage = ValidationMessages.PasswordComplexity)]
        public List<string> UserEmail { get; set; }
    }
}