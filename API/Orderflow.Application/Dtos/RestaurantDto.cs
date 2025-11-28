public class RestaurantDto
{
    public string Name { get; set; }
    public string Address { get; set; }
    public TimeSpan OpeningTime { get; set; }
    public TimeSpan ClosingTime { get; set; }
    public int? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Instagram { get; set; }
    // Owner info
    public string OwnerName { get; set; }
    public string OwnerEmail { get; set; }
}