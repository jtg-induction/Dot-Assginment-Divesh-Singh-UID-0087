using RestaurantManagement.Constants;
using RestaurantManagement.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RestaurantManagement.Models.Dto
{
    public class OrderRequestForOwner:PaginationParams
    {
        public OrderSortType sortby { get; set; } = OrderSortType.CreatedAt;
        public SortOrder sortOrder { get; set; } = SortOrder.desc;
        public SearchBY? searchby { get; set; }
        public string search { get; set; }
        public int? id { get; set; }
        public OrderStatus? Status { get; set; }
        public FilterBy? filterby { get; set; }
        [Range(1,int.MaxValue,ErrorMessage =ValidationMessages.MinAmountZero)]
        public int? minamount { get; set; }
        public int? maxamount { get; set; }
        public DateTime? mindate { get; set; }
        public DateTime? maxdate { get; set; }
    }
}