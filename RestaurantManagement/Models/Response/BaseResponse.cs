using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Response
{
    public class BaseResponse
    {
     [Required]   public DateTime CreatedAt { get; set; }
       [Required] public DateTime UpdatedAt { get; set; }
    }
}