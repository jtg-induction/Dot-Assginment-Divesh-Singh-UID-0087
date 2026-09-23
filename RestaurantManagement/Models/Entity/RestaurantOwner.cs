using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.Models.Entity
{
    /// <summary>
    /// Represents the relationship between a restaurant and its owner.
    /// </summary>
    public class RestaurantOwner:BaseEntity
    {
        [Key] public int OwnerId { get; set; }

        [Required] public int RestaurantId { get; set; }
        [ForeignKey("RestaurantId")] public  virtual Restaurant Restaurant { get; set; }

        [Required] public int UserId { get; set; }
        [ForeignKey("UserId")] public virtual User User { get; set; }
    }
}
