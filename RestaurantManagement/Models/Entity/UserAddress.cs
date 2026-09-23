using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantManagement.Models.Entity
{
    /// <summary>
    /// Represents the association between a user and an address.
    /// </summary>
    public class UserAddress
    {
    
        [Key] public int UserAddressId { get; set; }
        [Required] public int UserId { get; set; }
        [ForeignKey("UserId")] public User User { get; set; }
        [Required] public int AddressId { get; set; }
        [ForeignKey("AddressId")] public Address Address { get; set; }
    }
}
