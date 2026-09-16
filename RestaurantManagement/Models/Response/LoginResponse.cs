using RestaurantManagement.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Response
{
    public class LoginResponse
    {
            public string Token { get; set; }
            public string TokenType { get; set; } = "Bearer";
            public int ExpiresInSeconds { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
        
    }
}