using RestaurantManagement.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Dto
{
    /// <summary>
    ///This Request model help to onboard restaurant owners
    /// </summary>
    public class AddRestaurantOwnerRequest
    {
        [Required(ErrorMessage ="RestaurantEmail Required!!")]
        [EmailAddress(ErrorMessage = ValidationMessages.InvalidEmailFormat)]
        [RegularExpression(ValidationRules.EmailRegexPattern, ErrorMessage = ValidationMessages.InvalidEmailFormat)]    
        public string RestaurantEmail { get; set; }

        [Required(ErrorMessage = ValidationMessages.UserEmailRequired)]
        public List<string> UserEmail { get; set; }
    }
}