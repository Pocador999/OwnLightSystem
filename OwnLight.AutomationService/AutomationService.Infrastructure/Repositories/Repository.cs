using AutomationService.Domain.Interfaces;
using AutomationService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutomationService.Infrastructure.Repositories;

public class Repository<T>(DataContext dataContext) : IRepository<T>
    where T : class
{
    protected readonly DataContext _dataContext = dataContext;
    private readonly DbSet<T> _dbSet = dataContext.Set<T>();

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

    public async Task<IEnumerable<T>> GetAllAsync(int pageNumber, int pageSize)
    {
        var skipAmount = (pageNumber - 1) * pageSize;
        return await _dbSet.Skip(skipAmount).Take(pageSize).ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id) => await _dbSet.FindAsync(id);

    protected async Task SaveChangesAsync() => await _dataContext.SaveChangesAsync();

    public async Task<int> CountAsync() => await _dbSet.CountAsync();
}
