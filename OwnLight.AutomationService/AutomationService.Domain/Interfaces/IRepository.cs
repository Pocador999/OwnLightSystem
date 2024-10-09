namespace AutomationService.Domain.Interfaces;

public interface IRepository<T>
    where T : class
{
    Task<IEnumerable<T>> GetAllAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default
    );
    Task<T?> GetByIdAsync(Guid id);
    Task<T> CreateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T?> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T?> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
