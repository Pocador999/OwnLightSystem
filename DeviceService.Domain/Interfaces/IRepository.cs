namespace DeviceService.Domain.Interfaces;

public interface IRepository<T>
    where T : class
{
    Task<IEnumerable<T>> GetAllAsync(int page, int pageSize);
    Task<T?> GetByIdAsync(Guid id);
    Task<T> CreateAsync(T entity);
    Task<T?> UpdateAsync(T entity);
    Task<T?> DeleteAsync(Guid id);
    Task<int> CountAsync();
}
