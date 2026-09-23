using RestaurantManagement.Constants;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.Models.Entity
{
    /// <summary>
    /// Represents a restaurant registered in the system.
    /// </summary>
    public class Restaurant:BaseEntity
    {
        [Key] public int RestaurantId { get; set; }
        [Required] [StringLength(EntityConstants.MaxNameLength)]public string Name { get; set; }
        [Required] public int AddressId { get; set; }
        [ForeignKey("AddressId")] public virtual Address Address { get; set; }
        [Required] [StringLength(EntityConstants.MaxEmailLength)]  [Index(IsUnique =true)] public string Email { get; set; }
        [Required][StringLength(EntityConstants.MaxPhoneNumberLength)] [Index(IsUnique = true)] public string PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;


    }
}

