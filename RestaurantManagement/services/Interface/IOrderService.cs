using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Services.Interface
{
  public interface IOrderService
    {
        Task<OrderResponse> AddOrder(AddOrderRequest addOrder, int userid);
    }
}
