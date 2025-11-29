
namespace Orderflow.Core.Entities
{
    public abstract class Tenant
    {
        public Guid Id { get; set; }
        public Guid RestaurantId { get; set; }
    }
}