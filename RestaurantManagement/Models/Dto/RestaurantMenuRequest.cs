using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Dto
{
    public class RestaurantMenuRequest
    {
        [Required] public int RestaurantId { get; set; }
    }
}