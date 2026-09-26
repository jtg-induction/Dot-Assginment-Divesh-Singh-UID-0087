using RestaurantManagement.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Response
{
    public class GetPaginatedResponse<T>
    {
        public PaginationMetaData pagination { get; set; }
        public List<T> order { get; set; }
    }
}