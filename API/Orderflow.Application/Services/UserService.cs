using Orderflow.Core;
using Orderflow.Application.Interfaces;

namespace Orderflow.Application.Services
{
    public class UserService : IUserService
    {
        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task<User> AddUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}