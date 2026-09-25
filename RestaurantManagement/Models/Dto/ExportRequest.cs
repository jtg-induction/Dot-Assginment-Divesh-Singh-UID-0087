using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Dto
{
    public class ExportRequest
    {
        public string ReportId { get; set; }
        public string Format { get; set; } = "PDF";
        public List<int> Parameters { get; set; } = new List<int>(0);
    }
}