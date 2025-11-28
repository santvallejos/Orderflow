namespace Orderflow.Core;

public class Order
{
    private decimal _discount;
    private decimal _totalPrice;

    public Guid Id { get; set; }
    public Guid RestaurantId { get; set; }
    public List<MenuItem> OrderedItems { get; set; }
    public TimeSpan OrderTime { get; set; }
    public decimal Discount { 
        get => _discount; 
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Discount cannot be negative.");
            }
            else
            {
                _discount = value;
            }
        }
    }
    public decimal TotalPrice { 
        get => _totalPrice; 
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("Total price cannot be negative.");
            }
            else
            {
                _totalPrice = value;
            }
        }
    }
    public EnumOrderStatus Status { get; set; }
}