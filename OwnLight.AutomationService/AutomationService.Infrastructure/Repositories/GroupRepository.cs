using AutomationService.Domain.Entities;
using AutomationService.Domain.Interfaces;
using AutomationService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutomationService.Infrastructure.Repositories;

public class GroupRepository(DataContext dataContext)
    : Repository<Group>(dataContext),
        IGroupRepository
{
    private readonly DbSet<Group> _dbSet = dataContext.Set<Group>();

    public async Task<IEnumerable<Group>> GetUserGroupsAsync(Guid userId, int page, int pageSize)
    {
        var skipAmount = (page - 1) * pageSize;
        return await _dbSet
            .Where(g => g.UserId == userId)
            .Skip(skipAmount)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Group?> GetGroupByNameAsync(string name) =>
        await _dbSet.FirstOrDefaultAsync(g => g.Name == name);
}
