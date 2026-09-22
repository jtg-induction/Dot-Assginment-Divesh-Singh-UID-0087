using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Constants
{
    /// <summary>
    /// Defines validation limits used by restaurant management entities.
    /// </summary>
    public static class EntityConstants
    {
        public const int MaxNameLength = 255;
        public const int MinNameLength = 3;
        public const int MaxEmailLength = 254; //mailbox name length limit as per RFC 5321
        public const int MaxPhoneNumberLength = 15;
        public const int MaxDishNameLength = 183;
        public const int MaxAddressLength = 255;
        public const int MaxStreetLength = 100;
        public const int MaxCityLength = 168;
        public const int MaxStateLength = 50;
        public const int MaxPinCodeLength = 12;
        public const int MinCityLength = 1;
        public const int MinStateLength = 2;
        public const int MinAddressLength = 3;
        public const int MinPasswordLength = 8;
        public const int MaxPasswordLength = 100;
        public const int MinStreetLength = 2;
        public const string MaxDecimalLength = "79228162514264337593543950335";
    }
}

