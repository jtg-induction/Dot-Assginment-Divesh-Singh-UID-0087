using RestaurantManagement.Data;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BCrypt.Net;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Services;
using System.Web.Http.Results;
using System.Runtime.CompilerServices;
using RestaurantManagement.Repository;
using RestaurantManagement.services;

namespace RestaurantManagement.Controllers
{
    [RoutePrefix("api/signup")]
    public class SignUpController : ApiController
    {
        private readonly IUserService _userservice;
        

        public SignUpController(IUserService userser)
        {
            _userservice = userser;
        }
     

        private Boolean validatepassword(String pass)
        {

            String specialchar = "!@#$%^&*";
            Boolean lowercase = false;
           Boolean uppercase = false;
           Boolean numberic = false;
           Boolean specialcharacter = false;
          for(int i = 0; i < pass.Length; i++)
            {
                if (pass[i]>='a' && pass[i] <= 'z')
                {
                    lowercase=true;
                }
                else if (pass[i]>='A' && pass[i]<= 'Z')
                {
                    uppercase=true;
                }
                else if (pass[i]>='0' && pass[i] <= '9')
                {
                    numberic=true;
                }
                else if (specialchar.Contains(pass[i]))
                {
                    specialcharacter=true;
                }

            }
          if(pass.Length>=8 && lowercase && uppercase && numberic && specialcharacter)
            {
                return true;
            }
            return false;
        }
        private Boolean validateemail(string email)
        {
            return email != null && new EmailAddressAttribute().IsValid(email);
        }
        private Boolean validatephonenumber(string ph)
        {
            return ph!=null &&  new PhoneAttribute().IsValid(ph);
        }
        private Boolean validatename( String nm)
        {
            return nm!=null &&  nm.Length > 0;
        }
        private Boolean validatedate(DateTime birth)
        {
            if (birth!=DateTime.MinValue) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult signup(AddUserRequest adduser)
        {
            adduser.Password = adduser.Password.Trim();
            
            if (validateemail(adduser.Email) && validatepassword(adduser.Password) && validatephonenumber(adduser.PhoneNumber) && validatename(adduser.Name)  && validatedate(adduser.BirthDate))
            {
                var res = _userservice.Adduser(adduser);
                if (res.Equals("ok"))
                {
                    return Created("succes",adduser);
                }
                else
                {
                    return BadRequest(res);
                  
                }
             }
            else
            {
                return BadRequest("Nothing to be null and check email and password") ;
            }

        }
    }
}
