using RestaurantManagement.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Response
{
    /// <summary>
    /// while response for order detail for owner adding pagination metadata and thier order
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GetPaginatedResponse<T>
    {
        public PaginationMetaData pagination { get; set; }
        public List<T> orders { get; set; }
    }
}