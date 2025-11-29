
using Orderflow.Core.Interfaces;

namespace Orderflow.API.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITenantService tenantService)
        {
            if (context.Request.Headers.TryGetValue("X-Restaurant-Id", out var headerValue))
            {
                if (Guid.TryParse(headerValue, out var headerTenantId))
                {
                    tenantService.SetTenantId(headerTenantId);
                }
            }

            await _next(context);
        }
    }
}