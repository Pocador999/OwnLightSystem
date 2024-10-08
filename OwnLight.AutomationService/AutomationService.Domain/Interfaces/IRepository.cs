namespace AutomationService.Domain.Interfaces;

public interface IRepository<T>
    where T : class
{
    public Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize);
    public Task<T?> GetByIdAsync(Guid id);
    public Task<T> CreateAsync(T entity);
    public Task<T?> UpdateAsync(T entity);
    public Task<T?> DeleteAsync(Guid id);
}
