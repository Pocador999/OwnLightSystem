using AutomationService.Domain.Entities;
using AutomationService.Domain.Interfaces;
using AutomationService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutomationService.Infrastructure.Repositories;

public class RoutineRepository(DataContext dataContext)
    : Repository<Routine>(dataContext),
        IRoutineRepository
{
    private readonly DbSet<Routine> _dbSet = dataContext.Set<Routine>();

    public async Task<IEnumerable<Routine>> GetUserRoutinesAsync(Guid userId) =>
        await _dbSet.Where(r => r.UserId == userId).ToListAsync();

    public async Task<Routine?> GetRoutineByNameAsync(string name) =>
        await _dbSet.FirstOrDefaultAsync(r => r.Name == name);
}
