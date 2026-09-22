using RestaurantManagement.Constants;
using RestaurantManagement.Data;
using RestaurantManagement.Exceptions;
using RestaurantManagement.Models.Entity;
using RestaurantManagement.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace RestaurantManagement.Repository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used to access user records.</param>
        public MenuRepository(ApplicationDbContext context)
        {
            _db = context;
        }
        public async Task<List<MenuItem>> GetMenuItem(int id)
        {
            return await _db.MenuItems.Where(e => e.RestaurantId == id).ToListAsync();
        }
        public async Task<List<MenuItem>> GetItemDetail(Dictionary<int, int> item)
        {

            var ids = string.Join(",", item.Keys);
            List<MenuItem> menu = await _db.MenuItems.SqlQuery($"SELECT * FROM MenuItems WITH(UPDLOCK,ROWLOCK) WHERE ITEMID IN ({ids})").ToListAsync();

            if (menu.Count != item.Count)
            {
                throw new ResourceException(ValidationMessages.MenuListInvalid);
            }
            int prev = 0;
            foreach (MenuItem i in menu)
            {
                if (!(prev == 0 || prev == i.RestaurantId))
                {
                    throw new ResourceException(ValidationMessages.OneRestaurant);
                }
                if (item[i.ItemId] > i.AvailableQuantity)
                {
                    throw new ResourceException($"{i.DishName} Are Not Available !!");
                }
                i.AvailableQuantity -= item[i.ItemId];
            }
            await _db.SaveChangesAsync();
            return menu;
        }

    }
}