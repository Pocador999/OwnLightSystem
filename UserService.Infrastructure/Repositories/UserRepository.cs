using UserService.Domain.Entities;
using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public Task<User> RegisterAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdateAsync(Guid id, string name, string userName)
    {
        throw new NotImplementedException();
    }

    public Task<User> UpdatePasswordAsync(Guid id, string passwordHash)
    {
        throw new NotImplementedException();
    }

    public Task<bool?> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<User>> FindAllAsync(int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<int> CountAsync()
    {
        throw new NotImplementedException();
    }

    public Task<User?> FindByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User?> FindByUserNameAsync(string userName)
    {
        throw new NotImplementedException();
    }
}
