using RestaurantManagement.Data;
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
     
    }
}