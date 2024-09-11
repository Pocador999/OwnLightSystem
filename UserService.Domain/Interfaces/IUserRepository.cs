using UserService.Domain.Entities;

namespace UserService.Domain.Interfaces;

public interface IUserRepository
{
    public Task<User> UpdatePasswordAsync(Guid id, string passwordHash);
    public Task<User> UpdateAsync(Guid id, string name, string userName);
    public Task<bool?> DeleteAsync(Guid id);
    public Task<User> RegisterAsync(string name, string userName, string passwordHash);
    public Task<User?> FindByIdAsync(Guid id);
    public Task<User?> FindByUserNameAsync(string userName);
    public Task<IEnumerable<User>> FindAllAsync(int page, int pageSize);
    public Task<int> CountAsync();

    // public Task<User?> AuthenticateAsync(string userName, string passwordHash);   
}
