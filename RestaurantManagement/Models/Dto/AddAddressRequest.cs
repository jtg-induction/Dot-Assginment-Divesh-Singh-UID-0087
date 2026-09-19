using RestaurantManagement.Constants;
using RestaurantManagement.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Dto
{
    public class AddAddressRequest
    {
        [Required(ErrorMessage =ValidationMessages.StreetRequired)]
        [StringLength(EntityConstants.MaxStateLength ,MinimumLength = EntityConstants.MinStateLength,ErrorMessage =ValidationMessages.StreetLength)]
        public string Street { get; set; }
        [Required(ErrorMessage =ValidationMessages.CityRequired)]
        [StringLength(EntityConstants.MaxCityLength, MinimumLength =  EntityConstants.MinCityLength,ErrorMessage =ValidationMessages.CityLength)]
        public string City { get; set; }
        [Required(ErrorMessage = ValidationMessages.StateRequired)]
        [StringLength(EntityConstants.MaxStateLength, MinimumLength = EntityConstants.MinStateLength,ErrorMessage =ValidationMessages.StateLength)]
       public string State { get; set; }
        [Required(ErrorMessage =ValidationMessages.PinCodeRequired)]
        [StringLength(EntityConstants.MaxPinCodeLength,MinimumLength = EntityConstants.MinPinCodeLength,ErrorMessage =ValidationMessages.PincodeLength)]
        public string Pincode { get; set; }
        [Required(ErrorMessage = ValidationMessages.AddressTypeRequired)]
        public AddressType AddressType { get; set; }
        [Required(ErrorMessage =ValidationMessages.CountryRequired)]
        [StringLength(EntityConstants.MaxCountryLength,MinimumLength = EntityConstants.MinCountryLength,ErrorMessage =ValidationMessages.PincodeLength)]
        public string Country { get; set; }
    }
}