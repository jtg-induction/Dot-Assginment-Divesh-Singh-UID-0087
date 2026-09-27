using RestaurantManagement.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Enum
{
    /// <summary>
    /// this help in to sort the order by which column
    /// </summary>
    public enum OrderSortType
    {
        RestaurantName = 1,
        TotalAmount = 2,
        CreatedAt = 3,
        CustomerName=4,
        UpdatedAt=5
    }
}