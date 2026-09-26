using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Response
{
    public class GetOrderResponseForOwner
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public string RestaurantName { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}