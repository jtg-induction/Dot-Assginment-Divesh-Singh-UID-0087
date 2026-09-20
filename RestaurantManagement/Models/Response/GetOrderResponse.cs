using RestaurantManagement.Constants;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Response
{
    public class GetOrderResponse
    {
         public int OrderId { get; set; }
        public string RestaurantName { get; set; }
         public decimal TotalAmount { get; set; }
        public string Status { get; set; } 
        public string Address { get; set; }
    }
}