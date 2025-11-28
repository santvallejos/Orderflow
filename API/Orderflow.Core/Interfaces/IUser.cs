using Orderflow.Core;

public interface IUser
{
    Task<User> GetUserByIdAsync(Guid userId);
    Task<User> AddUserAsync(User user);
    Task<User> UpdateUserAsync(User user);
    Task<bool> DeleteUserAsync(Guid userId);
}