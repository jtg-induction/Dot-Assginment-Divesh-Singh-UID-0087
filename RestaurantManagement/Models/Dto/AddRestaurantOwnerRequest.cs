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
        public string RestaurantEmail { get; set; }

        [Required(ErrorMessage = ValidationMessages.UserEmailRequired)]
        public List<string> UserEmail { get; set; }
    }
}