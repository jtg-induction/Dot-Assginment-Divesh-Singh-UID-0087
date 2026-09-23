using RestaurantManagement.Constants;
using RestaurantManagement.Models.Enum;
using System;
using System.ComponentModel.DataAnnotations;


namespace RestaurantManagement.Models.Entity
{
    /// <summary>
    /// Represents a postal address associated with an entity.
    /// </summary>
    public class Address:BaseEntity
    {
        [Key] public int AddressId { get;set; }
        [Required][StringLength(EntityConstants.MaxStreetLength,MinimumLength =EntityConstants.MinStreetLength)] public string Street{ get; set; }
        [Required] [StringLength(EntityConstants.MaxCityLength,MinimumLength =EntityConstants.MinCityLength)] public string City { get; set; }
        [Required] [StringLength(EntityConstants.MaxStateLength,MinimumLength =EntityConstants.MinStateLength)]public string State { get; set; }
        [Required] [StringLength(EntityConstants.MaxPinCodeLength)]public string PinCode { get; set; }
        [Required] public string Country { get; set; }
        [Required] public AddressType AddressType { get; set; }
  
    }
}
