using RestaurantManagement.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Dto
{
    public class AddRestaurantRequest :AddAddressRequest
    {
        [Required(ErrorMessage =ValidationMessages.NameRequired)]
        [StringLength(EntityConstants.MaxNameLength, MinimumLength = EntityConstants.MinNameLength)]
        public string Name { get; set; }
        [Required(ErrorMessage = ValidationMessages.EmailRequired)]
        [EmailAddress(ErrorMessage = ValidationMessages.InvalidEmailFormat)]
        [RegularExpression(ValidationRules.PasswordRegexPattern, ErrorMessage = ValidationMessages.PasswordComplexity)]
        public string Email { get; set; }

        [Required(ErrorMessage = ValidationMessages.PhoneRequired)]
        [RegularExpression(ValidationRules.PhoneRegexPattern, ErrorMessage = ValidationMessages.InvalidPhoneFormat)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage =ValidationMessages.UserEmailRequired)]
        public List<string> UserEmail { get; set; }
    }
}