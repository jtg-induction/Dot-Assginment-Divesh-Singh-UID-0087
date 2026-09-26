using RestaurantManagement.Constants;
using RestaurantManagement.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Dto
{
    public class PaginationParams
    {
        [Range(1,int.MaxValue,ErrorMessage = ValidationMessages.PageNumberZero)]
        public int pageNumber { get; set; }
        [Range(1,int.MaxValue,ErrorMessage = ValidationMessages.PageSizeZero)]
        public int pageSize { get; set; }
   



    }
}