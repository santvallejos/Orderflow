using Orderflow.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Orderflow.Core.Entities;

namespace Orderflow.Infrastructure.Data
{
    public class OrderflowDbContext : DbContext
    {
        private readonly ITenantService _tenantService;

        public OrderflowDbContext(
            DbContextOptions<OrderflowDbContext> options,
            ITenantService tenantService
        ) : base(options)
        {
            _tenantService = tenantService;
        }

        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        // Configura el modelo y aplica filtros globales
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>()
                .HasQueryFilter(e => e.RestaurantId == _tenantService.GetTenantId());// Aplica filtro global para multi-tenant

            modelBuilder.Entity<Restaurant>()
                .HasIndex(r => r.Slug)
                .IsUnique();
        }

        // Sobrescribe SaveChangesAsync para establecer el RestaurantId automáticamente
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<Tenant>();

            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.RestaurantId = _tenantService.GetTenantId();
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}