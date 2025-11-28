using Orderflow.Core;
using Orderflow.Application.Interfaces;


namespace Orderflow.Application.Services
{
    public class MenuItemService : IMenuItemService
    {
        public async Task<MenuItem> GetMenuItemByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<MenuItem> GetMenuItemByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<MenuItem> GetMenuItemByCategoryAsync(string category)
        {
            throw new NotImplementedException();
        }
        public async Task<MenuItem> AddMenuItemAsync(MenuItem menuItem)
        {
            throw new NotImplementedException();
        }

        public async Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteMenuItemAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}