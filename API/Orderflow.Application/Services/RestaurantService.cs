using Orderflow.Core;
using Orderflow.Application.Interfaces;

namespace Orderflow.Application.Services
{
    public class RestaurantService : IRestaurantService
    {
        public async Task<Restaurant> GetRestaurantByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Restaurant> GetRestaurantByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public async Task<Restaurant> AddRestaurantAsync(Restaurant restaurant)
        {
            throw new NotImplementedException();
        }

        public async Task<Restaurant> UpdateRestaurantAsync(Restaurant restaurant)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteRestaurantAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}