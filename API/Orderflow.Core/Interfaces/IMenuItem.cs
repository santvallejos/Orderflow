using Orderflow.Core;

public interface IMenuItem
{
    Task<MenuItem> GetMenuItemByIdAsync(Guid id);
    Task<MenuItem> GetMenuItemByNameAsync(string name);
    Task<MenuItem> GetMenuItemByCategoryAsync(string category);
    Task<MenuItem> AddMenuItemAsync(MenuItem menuItem);
    Task<MenuItem> UpdateMenuItemAsync(MenuItem menuItem);
    Task<bool> DeleteMenuItemAsync(Guid id);
}