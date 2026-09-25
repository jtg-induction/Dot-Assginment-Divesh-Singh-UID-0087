using NMemory.Linq;
using RestaurantManagement.Data;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace RestaurantManagement.Repository
{
    public class OrderRepository :IOrderRepository
    {
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used to access user records.</param>
        public OrderRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public async Task PlacedOrder(Order order)
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
        }
        public async Task<List<Order>> GetOrder(int id)
        {
            return await _db.Orders.Include(e=>e.Restaurant).Where(e => e.UserId == id).ToListAsync();
        }
        public async Task<Order> GetOrderDetail(int id)
        {
            return await _db.Orders.FindAsync(id);
        }
       public async Task CancelOrder(Order order)
        {
            order.Status = OrderStatus.Cancelled;
            await _db.SaveChangesAsync();
        }
        public async Task<int> GetAllOrderByOwner(int id)
        {
            var query = from o in _db.Orders
                        join r in _db.Restaurants on o.RestaurantId equals r.RestaurantId
                        join ro in _db.RestaurantOwners on r.RestaurantId equals ro.RestaurantId
                        where ro.UserId==id
                        select o;
          return  await  query.CountAsync();
                      
        }
        public async Task<List<GetOrderResponseForOwner>> GetPaginatedOrder(PaginationParams paginationParams,int userid)
        {
            var query = from o in _db.Orders
                        join r in _db.Restaurants on o.RestaurantId equals r.RestaurantId
                        join ro in _db.RestaurantOwners on r.RestaurantId equals ro.RestaurantId
                        join u in _db.Users on o.UserId equals u.UserId
                        where ro.UserId == userid
                        select new
                        {
                            order = o,
                            RestuarantName = r.Name,
                            user=u
                        };

            //searching
            switch (paginationParams.searchby)
            {
                case SearchBY.CustomerName:
                    {
                        query = query.Where(e => e.user.Name.Contains(paginationParams.search));
                        break;
                    }
                case SearchBY.RestaurantName:
                    {
                        query = query.Where(e => e.RestuarantName.Contains(paginationParams.search));

                        break;
                    }
                case SearchBY.Address:
                {

                        query = query.Where(e => e.order.Address.Contains(paginationParams.search));

                        break;
            }
                case SearchBY.OrderId:
                    {

                        query = query.Where(e => e.order.OrderId==paginationParams.id);

                        break;
                    }
                case SearchBY.Status:
                    {

                        query = query.Where(e => e.order.Status==paginationParams.Status);

                        break;
                    }

            }
            //sorting
            switch (paginationParams.sortby)
            {
                case OrderSortType.CreatedAt:
                    {
                        if (paginationParams.sortOrder == SortOrder.asc)
                        {
                            query = query.OrderBy(e => e.order.CreatedAt);
                        }
                        else
                        {
                            query = query.OrderByDescending(e => e.order.CreatedAt);
                        }
                        break;
                    }
                case OrderSortType.TotalAmount:
                    {
                        if (paginationParams.sortOrder == SortOrder.asc)
                        {
                            query = query.OrderBy(e => e.order.TotalAmount);
                        }
                        else
                        {
                            query = query.OrderByDescending(e => e.order.TotalAmount);
                        }
                        break;
                    }

                case OrderSortType.CustomerName:
                    {


                        if (paginationParams.sortOrder == SortOrder.asc)
                        {
                            query = query.OrderBy(e => e.user.Name);
                        }
                        else
                        {
                            query = query.OrderByDescending(e => e.user.Name);
                        }
                        break;
                    }
                case OrderSortType.UpdatedAt:
                    {


                        if (paginationParams.sortOrder == SortOrder.asc)
                        {
                            query = query.OrderBy(e => e.order.UpdatedAt);
                        }
                        else
                        {
                            query = query.OrderByDescending(e => e.order.UpdatedAt);
                        }
                        break;
                    }

            }
            //filtering
            switch (paginationParams.filterby)
            {
                case FilterBy.TotalAmount:
                    {
                        if (paginationParams.filterorder == FilterOrder.lt)
                        {
                            query = query.Where(e => e.order.TotalAmount<paginationParams.amount);
                        }
                        else
                        {
                            query = query.Where(e => e.order.TotalAmount >=paginationParams.amount);
                        }
                        break;
                    }
                case FilterBy.CreatedAt:
                    {
                        if (paginationParams.filterorder == FilterOrder.lt)
                        {
                            query = query.Where(e => e.order.CreatedAt < paginationParams.date);
                        }
                        else
                        {
                            query = query.Where(e => e.order.CreatedAt >= paginationParams.date);
                        }
                        break;
                    }
                case FilterBy.UpdatedAt:
                    {
                        if (paginationParams.filterorder == FilterOrder.lt)
                        {
                            query = query.Where(e => e.order.UpdatedAt < paginationParams.date);
                        }
                        else
                        {
                            query = query.Where(e => e.order.UpdatedAt >= paginationParams.date);
                        }
                        break;
                    }
            }

            //pagination
            query = query.Skip((paginationParams.pageNumber - 1) * paginationParams.pageSize).Take(paginationParams.pageSize);
            return await query.Select(
                e => new GetOrderResponseForOwner
                {
                    OrderId = e.order.OrderId,
                    CustomerName=e.user.Name,
                    RestaurantName = e.RestuarantName,
                    TotalAmount = e.order.TotalAmount,
                    Status = e.order.Status.ToString(),
                    Address = e.order.Address,
                    CreatedAt=e.order.CreatedAt,
                    UpdatedAt=e.order.UpdatedAt
                   
                }
            ).ToListAsync();
        }
        public async Task UpdateOrderStatus(Order order,OrderStatus orderStatus)
        {
            order.Status = orderStatus;
            await _db.SaveChangesAsync();
        }
    }
}