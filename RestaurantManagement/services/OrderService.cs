using NMemory.Transactions;
using RestaurantManagement.Constants;
using RestaurantManagement.Exceptions;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Enum;
using RestaurantManagement.Models.Response;
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
    public class OrderService :IOrderService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        public OrderService(IMenuRepository menuRepository,IAddressRepository addressRepository,IUserRepository userRepository,IOrderRepository orderRepository,IOrderItemRepository orderItemRepository)
        {
            _menuRepository = menuRepository;
            _addressRepository = addressRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }
        public async Task<OrderResponse> AddOrder(Dictionary<int,int> item,int addressid,int userid)
        {
           
            var data=new OrderResponse();
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
            string address = await _addressRepository.GetAddress(addressid);
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
                data = new OrderResponse()
                {
                    OrderId = order.OrderId,
                    RestaurantId = order.RestaurantId,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status.ToString(),
                    Address = order.Address
                };
                transactionScope.Complete();
            }
            return data;
        }
        public async Task<List<Order>> GetOrder(int id)
        {
            return await _orderRepository.GetOrder(id);
        }
        public async Task<List<OrderItem>> GetOrderItem(int id)
        {
            return await _orderItemRepository.GetOrderItem(id);
        }
        public async Task OrderCancel(int id)
        {
            Order order =await _orderRepository.GetOrderDetail(id);
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
           await  _orderRepository.CancelOrder(order);
        }
    }
}