using Orderflow.Core.Entities;

namespace Orderflow.Core.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<Restaurant?> GetRestaurantByIdAsync(Guid id);
    }
}