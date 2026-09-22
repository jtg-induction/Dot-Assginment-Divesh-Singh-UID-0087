using RestaurantManagement.Constants;
using RestaurantManagement.Exceptions;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Repository;
using RestaurantManagement.Repository.Interface;
using RestaurantManagement.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantManagement.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        public MenuService(IMenuRepository menuRepository, IRestaurantRepository restaurantRepository)
        {
            _menuRepository = menuRepository;
            _restaurantRepository = restaurantRepository;
        }
        public async Task<List<GetMenuItemResponse>> GetMenuItemsAsync(int id)
        {
            if (!(await _restaurantRepository.RestaurantIsActive(id)))
            {
                throw new ResourceException(ValidationMessages.RestaurantNotFound);
            }
            List<MenuItem> menu = await _menuRepository.GetMenuItem(id);
            List<GetMenuItemResponse> menuitem = new List<GetMenuItemResponse>();
            foreach (MenuItem items in menu)
            {
                var item = new GetMenuItemResponse()
                {
                    ItemId = items.ItemId,
                    DishName = items.DishName,
                    Price = items.Price,
                    AvailableQuantity = items.AvailableQuantity,


                };
                menuitem.Add(item);

            }
            return menuitem;
        }
    }
}