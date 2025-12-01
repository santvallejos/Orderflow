using Orderflow.Core.Entities;
using Orderflow.Core.Interfaces;
using Orderflow.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orderflow.Infrastructure.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly OrderflowDbContext _context;

        public RestaurantRepository(OrderflowDbContext context)
        {
            _context = context;
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(Guid id)
        {
            return await _context.Restaurants.FindAsync(id);
        }
    }
}
