using Orderflow.Core;

public interface ITheme
{
    Task<Theme> GetThemeByIdAsync(Guid id);
    Task<Theme> GetThemeByRestaurantIdAsync(Guid restaurantId);
    Task<Theme> CreateThemeAsync(Theme theme);
    Task<Theme> UpdateThemeAsync(Theme theme);
    Task<bool> DeleteThemeAsync(Guid id);
}