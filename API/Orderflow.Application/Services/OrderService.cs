using Orderflow.Core;
using Orderflow.Application.Interfaces;

namespace Orderflow.Application.Services
{
    public class OrderService : IOrderService
    {
        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> GetOrderByRestaurantIdAsync(Guid restaurantId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MenuItem>> GetOrderItemsByIdAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<MenuItem>> GetOrderItemsByRestaurantIdAsync(Guid restaurantId)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> AddOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> AddItemToOrderAsync(Guid orderId, MenuItem item)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveItemFromOrderAsync(Guid orderId, MenuItem item)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteOrderAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}