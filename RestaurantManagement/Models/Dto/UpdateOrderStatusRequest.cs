using RestaurantManagement.Constants;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Dto
{
    public class UpdateOrderStatusRequest
    {
        [Required(ErrorMessage ="Order Id Required")]
        public int? OrderId { get; set; }
        [Required(ErrorMessage ="Provide the Order Status Updating")]
        public OrderStatus OrderStatus { get; set; }
    }
}