using Orderflow.Core;

namespace Orderflow.Application.Interfaces
{
    public interface IRestaurantService
    {
        Task<Restaurant> GetRestaurantByIdAsync(Guid id);
        Task<Restaurant> GetRestaurantByNameAsync(string name);
        Task<Restaurant> AddRestaurantAsync(Restaurant restaurant);
        Task<Restaurant> UpdateRestaurantAsync(Restaurant restaurant);
        Task<bool> DeleteRestaurantAsync(Guid id);
    }
}