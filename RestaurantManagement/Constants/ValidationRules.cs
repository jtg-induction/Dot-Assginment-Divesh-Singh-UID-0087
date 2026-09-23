using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Constants
{
    public class ValidationRules
    {
        public const string PhoneRegexPattern = @"^\d{10}$";
        public const string PasswordRegexPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";
    }
}