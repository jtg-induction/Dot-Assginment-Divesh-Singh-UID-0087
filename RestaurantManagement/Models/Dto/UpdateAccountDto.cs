using RestaurantManagement.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Dto
{
    public class UpdateAccountDto
    {
        [StringLength(EntityConstants.MaxNameLength, MinimumLength = EntityConstants.MinNameLength)]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        [EmailAddress(ErrorMessage = ValidationMessages.InvalidEmailFormat)]
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the user's birth date.
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the user's phone number.
        /// </summary>
        [RegularExpression(ValidationRules.PhoneRegexPattern, ErrorMessage = ValidationMessages.InvalidPhoneFormat)]
        public string PhoneNumber { get; set; }
    }
}