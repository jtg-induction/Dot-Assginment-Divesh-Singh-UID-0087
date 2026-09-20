using RestaurantManagement.Constants;
using RestaurantManagement.Models.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Response
{
    public class GetOrderItemResponse
    {
         public int OrderItemId { get; set; }
         public int ItemId { get; set; }
   public string ItemName { get; set; }
        public decimal Price { get; set; }
       public int Quantity { get; set; }
    }
}