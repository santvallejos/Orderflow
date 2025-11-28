using Orderflow.Core;
using Orderflow.Application.Interfaces;

namespace Orderflow.Application.Services
{
    public class ThemeService : IThemeService
    {
        public async Task<Theme> GetThemeByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Theme> GetThemeByRestaurantIdAsync(Guid restaurantId)
        {
            throw new NotImplementedException();
        }

        public async Task<Theme> AddThemeAsync(Theme theme)
        {
            throw new NotImplementedException();
        }

        public async Task<Theme> UpdateThemeAsync(Theme theme)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteThemeAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}