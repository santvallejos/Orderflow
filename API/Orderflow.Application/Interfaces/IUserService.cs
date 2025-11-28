using Orderflow.Core;

namespace Orderflow.Application.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(Guid userId);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid userId);
    }
}