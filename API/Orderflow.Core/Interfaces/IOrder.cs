using Orderflow.Core;

public interface IOrder
{
    Task<Order> GetOrderByIdAsync(Guid id);
    Task<Order> GetOrderByRestaurantIdAsync(Guid restaurantId);
    Task<List<MenuItem>> GetOrderItemsByIdAsync(Guid orderId);
    Task<List<MenuItem>> GetOrderItemsByRestaurantIdAsync(Guid restaurantId);
    Task<Order> AddOrderAsync(Order order);
    Task<Order> UpdateOrderAsync(Order order);
    Task<bool> AddItemToOrderAsync(Guid orderId, MenuItem item); // Agregar un item a la orden, si necesidad de actualizarlar por completo
    Task<bool> RemoveItemFromOrderAsync(Guid orderId, MenuItem item); // lo mismo para retirar un item
    Task<bool> DeleteOrderAsync(Guid id);
}