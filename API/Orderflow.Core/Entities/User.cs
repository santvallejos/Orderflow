namespace Orderflow.Core;

public class User
{
    public Guid Id { get; set; }
    public Guid RestaurantId { get; set; } // id al restaurante que pertenece el usuario
    public string Name { get; set; }
    public string Surname { get; set; }
    public EnumRole Role { get; set; }
}