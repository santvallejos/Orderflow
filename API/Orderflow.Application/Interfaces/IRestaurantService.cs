using Orderflow.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orderflow.Application.Interfaces
{
    public interface IRestaurantService
    {
        Task<RestaurantDto> GetRestaurantById(Guid id);
    }
}
