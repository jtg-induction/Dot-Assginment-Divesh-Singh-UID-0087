using RestaurantManagement.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantManagement.repository
{
    public interface IUserRepository
    {
        Boolean CheckEmailIsPresent(string email);
        Boolean CheckPhoneNumberIsPresent(String ph);
        String AddUser(User userentity);
    }
}