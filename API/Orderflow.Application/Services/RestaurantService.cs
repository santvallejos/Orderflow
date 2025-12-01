using Orderflow.Application.DTOs;
using Orderflow.Application.Interfaces;
using Orderflow.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orderflow.Application.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _repository; // Ocupamos la interfaz del repositorio porque application no debe conocer detalles de infraestructura

        public RestaurantService(IRestaurantRepository repository)
        {
            _repository = repository;
        }

        public async Task<RestaurantDto> GetRestaurantById(Guid id)
        {
            try
            {
                var restaurant = await _repository.GetRestaurantByIdAsync(id);
                if (restaurant == null)
                {
                    return null;
                }
                // Mapear la entidad Restaurant a RestaurantDto
                var restaurantDto = new RestaurantDto
                {
                    Name = restaurant.Name,
                    Slug = restaurant.Slug,
                    Menu = restaurant.Menu.Select(item => new MenuItemDto
                    {
                        Name = item.Name,
                        Description = item.Description,
                        Price = item.Price
                    }).ToList()
                };
                return restaurantDto;

            }
            catch (Exception ex)
            {
                // Manejo de errores (logging, rethrow, etc.)
                throw new ApplicationException("Error retrieving restaurant", ex);
            }
        }
    }
}
