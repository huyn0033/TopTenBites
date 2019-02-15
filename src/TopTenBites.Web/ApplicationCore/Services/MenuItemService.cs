using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TopTenBites.Web.ApplicationCore.Interfaces;
using TopTenBites.Web.ApplicationCore.Models;

namespace TopTenBites.Web.ApplicationCore.Services
{
    public class MenuItemService : IMenuItemService
    {
        private IGenericRepository<MenuItem> _menuItemRepository;

        public MenuItemService(IGenericRepository<MenuItem> menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        public MenuItem Get(int menuItemId)
        {
            return _menuItemRepository.Get(filter: x => x.MenuItemId == menuItemId,
                                    includeProperties: "Likes,Comments,Images");
        }

        public IEnumerable<MenuItem> GetAll(string menuItemIds = "")
        {
            IEnumerable<MenuItem> menuItems = Enumerable.Empty<MenuItem>();

            int[] aryMenuItemIds = menuItemIds?.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToArray();
            if (aryMenuItemIds?.Length > 0)
            {
                menuItems = _menuItemRepository.GetAll(filter: x => aryMenuItemIds.Contains((int)x.MenuItemId),
                                                    includeProperties: "Likes,Comments,Images");
            }
            else
            {
                menuItems = _menuItemRepository.GetAll(includeProperties: "Likes,Comments,Images");
            }

            return menuItems;
        }

        public MenuItem Add(MenuItem comment)
        {
            return _menuItemRepository.Add(comment);
        }

        public void Update(MenuItem comment)
        {
            _menuItemRepository.Update(comment);
        }
    }
}
