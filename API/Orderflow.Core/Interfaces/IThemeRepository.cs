using Orderflow.Core;

namespace Orderflow.Core.Interfaces
{
    public interface IThemeRepository
    {
        Task<Theme> GetThemeByIdAsync(Guid id);
        Task<Theme> GetThemeByRestaurantIdAsync(Guid restaurantId);
        Task<Theme> AddThemeAsync(Theme theme);
        Task<Theme> UpdateThemeAsync(Theme theme);
        Task<bool> DeleteThemeAsync(Guid id);
    }
}