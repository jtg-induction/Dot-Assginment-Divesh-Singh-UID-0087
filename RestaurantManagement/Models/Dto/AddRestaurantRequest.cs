using RestaurantManagement.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Dto
{
    public class AddRestaurantRequest
    {
        [Required(ErrorMessage =ValidationMessages.NameRequired)]
        public string Name { get; set; }
        [Required(ErrorMessage =ValidationMessages.AddressIdRequired)]
        public int AddressId { get; set; }
        [Required(ErrorMessage = ValidationMessages.EmailRequired)]
        [EmailAddress(ErrorMessage = ValidationMessages.InvalidEmailFormat)]
        public string Email { get; set; }

        [Required(ErrorMessage = ValidationMessages.PhoneRequired)]
        [RegularExpression(ValidationRules.PhoneRegexPattern, ErrorMessage = ValidationMessages.InvalidPhoneFormat)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage =ValidationMessages.UserIdRequired)]
        public List<int> UserId { get; set; }
    }
}