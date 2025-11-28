namespace Orderflow.Core;

public class MenuItem
{
    private decimal _price;

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public decimal Price { 
        get => _price; 
        set
            {
                if (value < 0) // No permitir precios negativos
                {
                    throw new ArgumentException("El precio no puede ser negativo.");
                }
                else
                {
                    _price = value;
                }
            }
        }
    public string? URLImage { get; set; }
}