namespace Orderflow.Core
{
    public class Theme // Personalizacion visual del sitio web gastronomico
    {
        public Guid Id { get; set; }
        public Guid RestaurantId { get; set; }
        public string PrimaryColor { get; set; } // Color principal del tema
        public string SecondaryColor { get; set; } // Color secundario del tema
        public string BackgroundColor { get; set; } // Color de fondo del tema
        public string FontFamily { get; set; } // Familia tipografica utilizada
    }
}