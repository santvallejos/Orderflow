
namespace Orderflow.Core.Entities
{
    public class Restaurant
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public List<MenuItem> Menu { get; set; } = new List<MenuItem>();
    }
}