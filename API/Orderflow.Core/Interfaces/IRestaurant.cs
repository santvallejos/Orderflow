using Orderflow.Core;

public interface IRestaurant
{
    Task<Restaurant> GetRestaurantByIdAsync(Guid id);
    Task<Restaurant> GetRestaurantByNameAsync(string name);
    Task<Restaurant> AddRestaurantAsync(Restaurant restaurant);
    Task<Restaurant> UpdateRestaurantAsync(Restaurant restaurant);
    Task<bool> DeleteRestaurantAsync(Guid id);
}