using NMemory.Transactions;
using RestaurantManagement.Constants;
using RestaurantManagement.Exceptions;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Models;
using RestaurantManagement.repository;
using RestaurantManagement.Repository;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace RestaurantManagement.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IRestaurantOwnerRepository _restaurantOwnerRepository;
        public OrderService(IMenuRepository menuRepository, IAddressRepository addressRepository, IUserRepository userRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository,IRestaurantRepository restaurantRepository,IRestaurantOwnerRepository restaurantOwnerRepository)
        {
            _menuRepository = menuRepository;
            _addressRepository = addressRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _restaurantRepository = restaurantRepository;
            _restaurantOwnerRepository = restaurantOwnerRepository;
        }
        public async Task<GetOrderResponse> AddOrder(Dictionary<int, int> item, int addressid, int userid)
        {
            if (item.Count < 0)
            {
                throw new InvalidOperationException(ValidationMessages.ItemRequired);
            }

            var data1 = new GetOrderResponse();
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var data = await _addressRepository.GetAddress(addressid);
                string address = $"{data.Street},{data.City},{data.State},{data.Country},{data.PinCode},{data.AddressType}";

                var menu = await _menuRepository.GetItemDetail(item);
                decimal totalamount = 0;
                foreach (MenuItem i in menu)
                {
                    totalamount += (i.Price * item[i.ItemId]);
                }

                await _userRepository.UpdateBalance(userid, totalamount);
                var order = new Order
                {
                    UserId = userid,
                    RestaurantId = menu[0].RestaurantId,
                    Address = address,
                    TotalAmount = totalamount

                };
                await _orderRepository.PlacedOrder(order);
                List<OrderItem> orderitems = new List<OrderItem>();
                foreach (MenuItem i in menu)
                {
                    orderitems.Add(new OrderItem()
                    {
                        OrderId = order.OrderId,
                        ItemId = i.ItemId,
                        ItemName = i.DishName,
                        Quantity = item[i.ItemId],
                        Price = i.Price
                    });

                }
                await _orderItemRepository.AddOrderItem(orderitems);
                data1 = new GetOrderResponse()
                {
                    OrderId = order.OrderId,
                    RestaurantName = await _restaurantRepository.GetRestaurantName(menu[0].RestaurantId),
                    TotalAmount = order.TotalAmount,
                    Status = order.Status.ToString(),
                    Address = order.Address
                };
                transactionScope.Complete();
            }

            return data1;
        }
        public async Task<List<GetOrderResponse>> GetOrder(int id)
        {
            var order= await _orderRepository.GetOrder(id);
            var data = new List<GetOrderResponse>();
            foreach (Order i in order)
            {
                data.Add(new GetOrderResponse
                {
                    OrderId = i.OrderId,
                    RestaurantName = await _restaurantRepository.GetRestaurantName(i.RestaurantId),
                    TotalAmount = i.TotalAmount,
                    Address = i.Address,
                    Status = i.Status.ToString()

                });
            }
            return data;
        }
        public async Task<List<GetOrderItemResponse>> GetOrderItem(int id)
        {
            var order = await _orderItemRepository.GetOrderItem(id);
            var data = new List<GetOrderItemResponse>();
            foreach (OrderItem i in order)
            {
                data.Add(new GetOrderItemResponse
                {
                    OrderItemId = i.OrderItemId,
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    Price = i.Price,
                    Quantity = i.Quantity

                });
            }
            return data;
        }
        public async Task OrderCancel(int id,int userid)
        {
            Order order = await _orderRepository.GetOrderDetail(id);
            if (userid != order.UserId)
            {
                throw new ResourceException(ValidationMessages.OrderMismatch);
            }
            if (order.Status == OrderStatus.Cancelled)
            {
                throw new ResourceException(ValidationMessages.OrderCancelled);
            }
            if (order.Status == OrderStatus.Rejected)
            {
                throw new ResourceException(ValidationMessages.OrderRejected);
            }
            if (order.Status == OrderStatus.Dispatched)
            {
                throw new ResourceException(ValidationMessages.OrderDispatched);
            }
            if (order.Status == OrderStatus.Delivery)
            {
                throw new Exception(ValidationMessages.OrderDelivered);
            }
            await _orderRepository.CancelOrder(order);
            await _userRepository.UpdateBalanceWhileCancelOrder(userid,order.TotalAmount);

        }
        public async Task<GetPaginatedResponse<GetOrderResponseForOwner>> GetAllOrder(PaginationParams paginationParams,int id)
        {
            var data = await _orderRepository.GetPaginatedOrder(paginationParams,id);
            var size = data.Count();
            var metadata = new PaginationMetaData
            {
                TotalItems = size,
                TotalPages = (int)Math.Ceiling((double)size / paginationParams.pageSize),
                CurrentPage = paginationParams.pageNumber,
                PageSize = paginationParams.pageSize
            };
            var data1 = new GetPaginatedResponse<GetOrderResponseForOwner>
            {
                pagination = metadata,
                order = data
            };
            return data1;
        }
    }
}