using RestaurantManagement.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.Models.Entity
{
    /// <summary>
    /// Represents a menu item offered by a restaurant.
    /// </summary>
    public class MenuItem : BaseEntity
    {
        [Key] public int ItemId { get; set; }
        [Required] public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")] public virtual Restaurant Restaurant { get; set; }
        [Required][StringLength(EntityConstants.MaxDishNameLength)] public string DishName { get; set; }
        [Required][Range(typeof(decimal), "1.0", EntityConstants.MaxDecimalLength)] public decimal Price { get; set; }
        [Required][Range(0, int.MaxValue)] public int AvailableQuantity { get; set; }

    }
}

