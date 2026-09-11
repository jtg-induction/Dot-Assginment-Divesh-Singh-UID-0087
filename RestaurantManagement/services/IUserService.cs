using RestaurantManagement.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.services
{
    public interface IUserService
    {
        string Adduser(AddUserRequest adduser);
    }
}
