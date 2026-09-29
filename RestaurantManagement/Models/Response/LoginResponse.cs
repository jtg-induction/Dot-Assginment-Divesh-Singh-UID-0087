using RestaurantManagement.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Response
{
    /// <summary>
    /// while login it give response of access token 
    /// </summary>
    public class LoginResponse
    {
        public string Token { get; set; }
    }
}