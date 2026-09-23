using RestaurantManagement.Constants;
using RestaurantManagement.Models.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.Models.Entity
{
    /// <summary>
    /// Represents a restaurant order placed by a user.
    /// </summary>
    public class Order:BaseEntity
    {
        [Key] public int OrderId { get; set; }
        [Required] public int UserId { get; set; }
        [ForeignKey("UserId")] public virtual User User { get; set; }
        [Required] public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")] public virtual Restaurant Restaurant { get; set; }
        [Required][Range(typeof(decimal), "1.0", EntityConstants.MaxDecimalLength)] public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Placed;
        [Required] [StringLength(EntityConstants.MaxAddressLength,MinimumLength =EntityConstants.MinAddressLength)] public string Address { get; set; }
    }
}

