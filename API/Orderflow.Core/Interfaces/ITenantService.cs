
namespace Orderflow.Core.Interfaces
{
    public interface ITenantService
    {
        Guid GetTenantId();
        void SetTenantId(Guid tenantId);
    }
}