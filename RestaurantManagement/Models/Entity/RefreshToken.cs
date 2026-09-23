using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Entity
{
    public class RefreshToken:BaseEntity
    {
        [Key]public int TokenId { get; set; }
        [Required] public int UserId { get; set; }
        [ForeignKey("UserId")] public virtual User User { get; set; }
        [Required] public string Token { get; set; }
        public bool IsRevoked { get; set; } = false;
        

    }
}