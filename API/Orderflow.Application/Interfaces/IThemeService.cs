using Orderflow.Core;

namespace Orderflow.Application.Interfaces
{
    public interface IThemeService
    {
        Task<Theme> GetThemeByIdAsync(Guid id);
        Task<Theme> GetThemeByRestaurantIdAsync(Guid restaurantId);
        Task<Theme> AddThemeAsync(Theme theme);
        Task<Theme> UpdateThemeAsync(Theme theme);
        Task<bool> DeleteThemeAsync(Guid id);
    }
}