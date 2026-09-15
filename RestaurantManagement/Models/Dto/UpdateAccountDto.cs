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
        [Required]
        [StringLength(EntityConstants.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(EntityConstants.MaxEmailLength)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [StringLength(EntityConstants.MaxPhoneNumberLength)]
        public string PhoneNumber { get; set; }
    }
}