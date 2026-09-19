using RestaurantManagement.Models.Entity;
using RestaurantManagement.Models.Response;
using RestaurantManagement.Repository;
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
        private readonly MenuRepository _menuRepository;
        public MenuService(MenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }
        public async Task<List<GetMenuItemResponse>> GetMenuItemsAsync(int id)
        {
            List<MenuItem> menu = await _menuRepository.GetMenuItem(id);
            List<GetMenuItemResponse> menuitem = new List<GetMenuItemResponse>();
            foreach (MenuItem i in menu)
            {
                var item = new GetMenuItemResponse()
                {
                    ItemId = i.ItemId,
                    DishName = i.DishName,
                    Price = i.Price,
                    AvailableQuantity = i.AvailableQuantity,
                  
                };
                menuitem.Add(item);

            }
            return menuitem;
        }
    }
}