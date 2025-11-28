using Orderflow.Core;

namespace Orderflow.Core.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<Restaurant> GetRestaurantByIdAsync(Guid id);
        Task<Restaurant> GetRestaurantByNameAsync(string name);
        Task<Restaurant> AddRestaurantAsync(Restaurant restaurant);
        Task<Restaurant> UpdateRestaurantAsync(Restaurant restaurant);
        Task<bool> DeleteRestaurantAsync(Guid id);
    }
}