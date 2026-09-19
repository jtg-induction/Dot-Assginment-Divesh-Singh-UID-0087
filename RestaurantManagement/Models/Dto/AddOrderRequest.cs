using RestaurantManagement.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Dto
{
    public class AddOrderRequest
    {
        [Required(ErrorMessage = ValidationMessages.ItemRequired)]
       public  Dictionary<int,int> ItemAndQuantity { get; set; }
        [Required(ErrorMessage = ValidationMessages.AddressIdRequired)]
        public int AddressId { get; set; }

    }
}