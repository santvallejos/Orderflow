using Orderflow.Core.Interfaces;

namespace Orderflow.Infrastructure.Services
{
    public class TenantService : ITenantService
    {
        private Guid _tenantId;

        public Guid GetTenantId() => _tenantId;
        public void SetTenantId(Guid tenantId) => _tenantId = tenantId;
    }
}