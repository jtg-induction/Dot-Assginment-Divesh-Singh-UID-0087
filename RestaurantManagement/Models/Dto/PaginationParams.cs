using RestaurantManagement.Models.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Dto
{
    public class PaginationParams
    {
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
       public OrderSortType sortby { get; set; } = OrderSortType.CreatedAt;
       public  SortOrder sortOrder { get; set; } = SortOrder.desc;
       public  SearchBY searchby{ get; set; }
        public string search { get; set; }
        public int id { get; set; }
        public OrderStatus Status { get; set; }
        public FilterBy filterby { get; set; }
        public FilterOrder filterorder { get; set; }
        public int amount { get; set; }
        public DateTime date { get; set; }



    }
}