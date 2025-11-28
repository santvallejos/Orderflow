namespace Orderflow.Core
{
    public class Restaurant
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; } // URL del restaurante
        public string Address { get; set; }
        public TimeSpan OpeningTime { get; set; } // Horario de apertura
        public TimeSpan ClosingTime { get; set; } // Horario de cierre
        public int? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Instagram { get; set; }

        // Owner info
        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }

        // Estado
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // Relaciones
        public Theme Theme { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<MenuItem> Menu { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}