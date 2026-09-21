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
        [Required(ErrorMessage ="RestaurantId Required!!")]
        public int RestaurantId { get; set; }

        [Required(ErrorMessage = ValidationMessages.UserIdRequired)]
        public List<int> UserId { get; set; }
    }
}