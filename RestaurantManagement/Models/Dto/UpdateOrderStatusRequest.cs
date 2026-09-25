using RestaurantManagement.Constants;
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
        [Required(ErrorMessage =ValidationMessages.OrderIdRequired)]
        public int OrderId { get; set; }
        [Required(ErrorMessage =ValidationMessages.OrderStatusUpdate)]
        public OrderStatus OrderStatus { get; set; }
    }
}