using Microsoft.AspNetCore.Mvc;
using Orderflow.Application.Interfaces;

namespace Orderflow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantsController : Controller
    {
        private readonly IRestaurantService _service;

        public RestaurantsController(IRestaurantService service)
        {
            _service = service;
        }

        [HttpGet("id")]
        public async Task<ActionResult> GetRestaurantById(Guid id)
        {
            var restaurant = await _service.GetRestaurantById(id);
            return Ok(restaurant);
        }
    }
}
