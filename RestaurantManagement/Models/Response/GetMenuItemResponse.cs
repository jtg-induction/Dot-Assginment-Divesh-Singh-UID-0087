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
    public class GetMenuItemResponse :BaseResponse
    {
        [Required] public int ItemId { get; set; }
        [Required][StringLength(EntityConstants.MaxDishNameLength)] public string DishName { get; set; }
        [Required][Range(typeof(decimal), "1.0", EntityConstants.MaxDecimalLength)] public decimal Price { get; set; }
        [Required][Range(0, int.MaxValue)] public int AvailableQuantity { get; set; }
    }
}