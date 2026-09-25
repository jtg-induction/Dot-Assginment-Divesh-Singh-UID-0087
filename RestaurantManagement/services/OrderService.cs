using NMemory.Transactions;
using RestaurantManagement.Constants;
using RestaurantManagement.Exceptions;
using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
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
        public async Task<OrderResponse> AddOrder(AddOrderRequest addOrder,int userid)
        {
            var item = addOrder.ItemAndQuantity;
            int? addressid = addOrder.AddressId;
            if (item.Count < 0)
            {
                throw new InvalidOperationException(ValidationMessages.ItemRequired);
            }
            foreach(int i in item.Keys)
            {
                if (item[i] <= 0)
                {
                    throw new InvalidOperationException(ValidationMessages.ItemIdZero);

                }
            }
           
            var orderResponse=new OrderResponse();
            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
            var data = await _addressRepository.GetAddressAsync((int)addressid,userid);
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
                        if (!menu.Any(e=>e.ItemId==i))
                        {
                            notfound+=$"Item Id {i} Not Found!!,";
                        }
                    }
                    throw new NotFoundException(notfound);
                }
                decimal totalamount = 0;
                foreach (MenuItem i in menu)
                {
                    
                    if ((addOrder.RestaurantId != i.RestaurantId))
                    {
                        throw new ResourceException(ValidationMessages.OneRestaurant);
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
    }
}