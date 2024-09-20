using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Data;

namespace UserService.Infrastructure.Repositories;

public class AuthRepository(DataContext context) : IAuthRepository
{
    private readonly DbSet<User> _dbSet = context.Set<User>();

    public async Task<User?> LoginAsync(string username, string password)
    {
        var user = await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        user!.Login();
        await context.SaveChangesAsync();
        return user;
    }

    public async Task<User?> LogoutAsync(Guid id)
    {
        var user = await _dbSet.FindAsync(id);
        if (user != null && user.Username != "admin")
        {
            user.Logout();
            await context.SaveChangesAsync();
            return user;
        }
        return null;
    }

    public async Task<IEnumerable<User>> GetAllLogedInUsersAsync() =>
        await _dbSet.Where(u => u.IsLogedIn && u.Username != "admin").ToListAsync();
}
