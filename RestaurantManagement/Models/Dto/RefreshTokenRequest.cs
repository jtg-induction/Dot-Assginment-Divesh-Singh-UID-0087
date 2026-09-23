using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Dto
{
    public class RefreshTokenRequest
    {
        [Required] public String Token { get; set; }
    }
}