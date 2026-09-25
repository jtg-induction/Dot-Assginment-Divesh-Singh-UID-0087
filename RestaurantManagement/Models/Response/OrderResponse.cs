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
    public class OrderResponse
    {
         public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
       
    }
}