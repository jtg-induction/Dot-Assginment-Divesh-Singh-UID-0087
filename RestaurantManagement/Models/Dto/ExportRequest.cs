using DocumentFormat.OpenXml.Office2010.ExcelAc;
using RestaurantManagement.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models.Dto
{
    public class ExportRequest
    {
        [RegularExpression(ValidationRules.FormatRegexPattern, ErrorMessage =ValidationMessages.InvalidFormat)]
        public string Format { get; set; } = "PDF";

        public string OrderId { get; set; } ="";

        public string RestaurantId { get; set; } = "";

    }
}
