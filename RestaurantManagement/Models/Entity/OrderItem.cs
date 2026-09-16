
using RestaurantManagement.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.Models.Entity
{
    /// <summary>
    /// Represents an item included in a customer order.
    /// </summary>
    public class OrderItem:BaseEntity
    {
        [Key] public int OrderItemId { get; set; }
        [Required] public int OrderId { get; set;}
        [ForeignKey("OrderId")] public virtual Order Order { get; set; }
        [Required] public int ItemId { get; set; } 
        [Required] [StringLength(EntityConstants.MaxDishNameLength)] public string ItemName { get; set; }
        [Required][Range(typeof(decimal), "1.0", EntityConstants.MaxDecimalLength)]public decimal Price { get; set; }
        [Required] [Range(1,int.MaxValue)]public int Quantity { get; set; }
       
    }
}
