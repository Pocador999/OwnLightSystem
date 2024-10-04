using DeviceService.Domain.Interfaces;
using DeviceService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeviceService.Infrastructure.Repositories;

public class Repository<T> : IRepository<T>
    where T : class
{
    protected readonly DataContext _dataContext;
    private readonly DbSet<T> _dbSet;

    public Repository(DataContext dataContext)
    {
        _dataContext = dataContext;
        _dbSet = _dataContext.Set<T>();
    }

    protected async Task SaveChangesAsync() => await _dataContext.SaveChangesAsync();

    public async Task<T> CreateAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await SaveChangesAsync();
        return entity;
    }

    public async Task<T?> UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await SaveChangesAsync();
        return entity;
    }

    public async Task<T?> DeleteAsync(Guid id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is null)
            return null;
        _dbSet.Remove(entity);
        await SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<T>> GetAllAsync(int page, int pageSize)
    {
        var skipAmount = (page - 1) * pageSize;
        return await _dbSet.Skip(skipAmount).Take(pageSize).ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

    public async Task<int> CountAsync() => await _dbSet.CountAsync();
}
