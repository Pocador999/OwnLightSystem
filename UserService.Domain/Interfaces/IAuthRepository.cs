using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces;

public interface IAuthRepository
{
    public Task<User?> LoginAsync(string username, string password);
    public Task<User?> LogoutAsync(Guid id);
}
