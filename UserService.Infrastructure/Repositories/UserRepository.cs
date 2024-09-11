using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.Repositories;

public class UserRepository(DataContext context) : IUserRepository
{
    private readonly DbSet<User> _dbSet = context.Set<User>();

    public async Task<User> RegisterAsync(User user)
    {
        await _dbSet.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateAsync(User user)
    {
        _dbSet.Update(user);
        await context.SaveChangesAsync();
        return user;
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

    public async Task<User?> FindByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public Task<User?> FindByUserNameAsync(string userName)
    {
        throw new NotImplementedException();
    }
}
