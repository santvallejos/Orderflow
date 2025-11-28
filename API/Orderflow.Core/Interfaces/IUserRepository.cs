using Orderflow.Core;

namespace Orderflow.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid userId);
    }
}