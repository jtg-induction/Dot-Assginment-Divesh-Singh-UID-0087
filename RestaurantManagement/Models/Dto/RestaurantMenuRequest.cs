using RestaurantManagement.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Dto
{
    public class RestaurantMenuRequest
    {
        [Required(ErrorMessage =ValidationMessages.RestaurantIdRequired)] public int RestaurantId { get; set; }
    }
}