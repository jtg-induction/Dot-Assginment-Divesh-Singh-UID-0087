using DocumentFormat.OpenXml.Office2010.ExcelAc;
using RestaurantManagement.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantManagement.Models.Dto
{
    public class ExportRequest
    {
        [Required(ErrorMessage = ValidationMessages.ExportFormat)]
        [RegularExpression(ValidationRules.FormatRegexPattern, ErrorMessage =ValidationMessages.InvalidFormat)]
        public string Format { get; set; } = "PDF";

        [Required(ErrorMessage = ValidationMessages.OrderIdRequird)]
        public string OrderId { get; set; } = string.Empty;

        [Required(ErrorMessage = ValidationMessages.RestaurantIdRequired)]
        public string RestaurantId { get; set; } = string.Empty;

    }
}
