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
using RestaurantManagement.Services.Exceptions;
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
        public OrderService(IMenuRepository menuRepository, IAddressRepository addressRepository, IUserRepository userRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IRestaurantRepository restaurantRepository, IRestaurantOwnerRepository restaurantOwnerRepository)
        {
            _menuRepository = menuRepository;
            _addressRepository = addressRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _restaurantRepository = restaurantRepository;
            _restaurantOwnerRepository = restaurantOwnerRepository;
        }
        public async Task<OrderResponse> AddOrder(AddOrderRequest addOrder, int userid)
        {
            var item = addOrder.ItemAndQuantity;
            int? addressid = addOrder.AddressId;
            if (item.Count < 0)
            {
                throw new InvalidOperationException(ValidationMessages.ItemRequired);
            }
            foreach (int i in item.Keys)
            {
                if (item[i] <= 0)
                {
                    throw new InvalidOperationException(ValidationMessages.ItemIdZero);

                }
            }

            var orderResponse = new OrderResponse();
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var data = await _addressRepository.GetAddressAsync((int)addressid, userid);
                if (data == null)
                {
                    throw new NotFoundException(ValidationMessages.InvalidAddress);
                }
                string address = $"{data.Street},{data.City},{data.State},{data.Country},{data.PinCode},{data.AddressType}";

                var menu = await _menuRepository.GetItemDetail(item);
                if (menu.Count != item.Count)
                {
                    string notfound = "";
                    foreach (int i in item.Keys)
                    {
                        if (!menu.Any(e => e.ItemId == i))
                        {
                            notfound += $"Item Id {i} Not Found!!,";
                        }
                    }
                    throw new NotFoundException(notfound);
                }
                decimal totalamount = 0;
                foreach (MenuItem i in menu)
                {

                    if ((addOrder.RestaurantId != i.RestaurantId))
                    {
                        throw new InvalidOperationException(ValidationMessages.OneRestaurant);
                    }
                    if (item[i.ItemId] > i.AvailableQuantity)
                    {
                        throw new NotFoundException($"{i.DishName} Are Not Available !!");
                    }
                    i.AvailableQuantity -= item[i.ItemId];
                    totalamount += (i.Price * item[i.ItemId]);
                }
                var user = await _userRepository.GetUserWithLock(userid);
                if (user.Balance < totalamount)
                {
                    throw new ResourceException(ValidationMessages.InsufficientBalance);
                }
                user.Balance -= totalamount;
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
                orderResponse = new OrderResponse()
                {
                    OrderId = order.OrderId,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status.ToString()
                };
                transactionScope.Complete();
            }

            return orderResponse;
        }
        public async Task<List<GetOrderResponse>> GetOrder(int id)
        {
            var order = await _orderRepository.GetOrder(id);
            var data = new List<GetOrderResponse>();
            foreach (Order i in order)
            {
                data.Add(new GetOrderResponse
                {
                    OrderId = i.OrderId,
                    RestaurantName = i.Restaurant.Name,
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
        public async Task OrderCancel(int id, int userid)
        {
            using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                Order order = await _orderRepository.GetOrderDetail(id);
                if (order == null)
                {
                    throw new NotFoundException(ValidationMessages.OrderNotFound);
                }
                if (userid != order.UserId)
                {
                    throw new InvalidOperationException(ValidationMessages.OrderMismatch);
                }
                if (order.Status == OrderStatus.Cancelled)
                {
                    throw new InvalidOperationException(ValidationMessages.OrderCancelled);
                }
                if (order.Status == OrderStatus.Rejected)
                {
                    throw new InvalidOperationException(ValidationMessages.OrderRejected);
                }
                if (order.Status == OrderStatus.Dispatched)
                {
                    throw new InvalidOperationException(ValidationMessages.OrderDispatched);
                }
                if (order.Status == OrderStatus.Delivery)
                {
                    throw new InvalidOperationException(ValidationMessages.OrderDelivered);
                }
                List<OrderItem> orderItems = await _orderItemRepository.GetOrderItem(order.OrderId);
                Dictionary<int, int> item = new Dictionary<int, int>();
                foreach (OrderItem i in orderItems)
                {
                    item[i.ItemId] = i.Quantity;
                }
                await _orderRepository.CancelOrder(order);
                await _userRepository.UpdateBalanceWhileCancelOrder(userid, order.TotalAmount);
                List<MenuItem> menu = await _menuRepository.GetItemDetail(item);
                foreach (MenuItem i in menu)
                {
                    i.AvailableQuantity += item[i.ItemId];
                }
                transaction.Complete();

            }

        }
        public async Task<GetPaginatedResponse<GetOrderResponseForOwner>> GetAllOrder(PaginationParams paginationParams, int id)
        {
            var data = await _orderRepository.GetPaginatedOrder(paginationParams, id);
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
        public async Task UpdateStatus(UpdateOrderStatusRequest updateOrderStatus,int userid)
        {
            Order order = await _orderRepository.GetOrderDetail((int)updateOrderStatus.OrderId);
            if (order == null || !(await _restaurantOwnerRepository.GetRestaurantId(userid)).Contains((int)updateOrderStatus.OrderId))
            {
                throw new   NotFoundException(ValidationMessages.OrderNotFound);
            }
            bool call = false;
            switch (updateOrderStatus.OrderStatus)
            {
                case OrderStatus.Accepted:
                    {

                        if (order.Status == OrderStatus.Placed)
                        {
                            call = true;

                        }
                        break;
                    }
                case OrderStatus.Rejected:
                    {

                        if (order.Status == OrderStatus.Placed)
                        {
                            call = true;
                        }
                        break;
                    }
                case OrderStatus.Dispatched:
                    {

                        if (order.Status == OrderStatus.Accepted)
                        {
                            call = true;
                        }
                        break;
                    }
                case OrderStatus.Delivery:
                    {

                        if (order.Status == OrderStatus.Dispatched)
                        {
                            call = true;
                        }
                        break;
                    }

                default:

                    throw new ResourceException($"Can Not Change the  Status From {order.Status.ToString()} to {updateOrderStatus.OrderStatus.ToString()}");

            }
            if (call)
            {

                await _orderRepository.UpdateOrderStatus(order, updateOrderStatus.OrderStatus);
            }

        }
    }
}
