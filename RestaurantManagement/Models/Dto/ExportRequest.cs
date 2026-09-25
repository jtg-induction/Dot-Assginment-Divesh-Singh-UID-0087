using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Dto
{
    public class ExportRequest
    {
        public string Format { get; set; } = "PDF";
        public string Parameters { get; set; } = "";
    }
}