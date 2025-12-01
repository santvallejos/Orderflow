using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orderflow.Application.DTOs
{
    public class RestaurantDto
    {
        public string Name { get; set; }
        public string Slug { get; set; }
        public List<MenuItemDto> Menu { get; set; }
    }
}
