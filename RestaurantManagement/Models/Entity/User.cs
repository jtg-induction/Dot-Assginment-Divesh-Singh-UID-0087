using RestaurantManagement.Models.Enum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestaurantManagement.Constants;
namespace RestaurantManagement.Models.Entity
{
        /// <summary>
        /// Represents a user account in the restaurant management system.
        /// </summary>
        public class User : BaseEntity
        {
                [Key] public int UserId { get; set; }
                [Required][StringLength(EntityConstants.MaxNameLength)] public string Name { get; set; }
                [Required] public string Password { get; set; }
                [Required][StringLength(EntityConstants.MaxEmailLength)][Index(IsUnique = true)] public string Email { get; set; }
                [Required][Column(TypeName = "date")] public DateTime BirthDate { get; set; }
                public bool IsActive { get; set; } = true;
                [Required][StringLength(EntityConstants.MaxPhoneNumberLength)][Index(IsUnique = true)] public string PhoneNumber { get; set; }
                [Required][Range(typeof(decimal), "0.0", EntityConstants.MaxDecimalLength)] public decimal Balance { get; set; } = 1000;
                [Required] public UserRole Role { get; set; }
                public DateTime BalanceUpdatedAt { get; set; } = DateTime.UtcNow;



        }
}

