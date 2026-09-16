using RestaurantManagement.Constants;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Response
{
    public class CreatedUserResponse:BaseEntity
    {
        [Required]public int UserId { get; set; }
        [Required]public string Name { get; set; }
        [Required]public string Email { get; set; }
        [Required]public DateTime BirthDate { get; set; }
        [Required]public string PhoneNumber { get; set; }
    }
}