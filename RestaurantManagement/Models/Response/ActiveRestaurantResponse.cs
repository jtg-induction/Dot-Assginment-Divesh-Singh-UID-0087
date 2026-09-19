using RestaurantManagement.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Response
{
    public class ActiveRestaurantResponse
    {
        public int RestaurantId { get; set; }
       public string Name { get; set; }
        public int AddressId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

    }
}