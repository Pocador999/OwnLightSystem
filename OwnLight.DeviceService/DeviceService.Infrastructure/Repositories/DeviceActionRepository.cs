using DeviceService.Domain.Entities;
using DeviceService.Domain.Interfaces;
using DeviceService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeviceService.Infrastructure.Repositories;

public class DeviceActionRepository(DataContext dataContext)
    : Repository<DeviceAction>(dataContext),
        IDeviceActionRepository
{
    private readonly DbSet<DeviceAction> _dbSet = dataContext.Set<DeviceAction>();

    public async Task<DeviceAction> AddDeviceActionAsync(DeviceAction deviceAction)
    {
        await _dbSet.AddAsync(deviceAction);
        await SaveChangesAsync();
        return deviceAction;
    }

    public async Task<IEnumerable<DeviceAction>> GetUserActionsAsync(
        Guid userId,
        int pageNumber,
        int pageSize
    )
    {
        var skipAmount = (pageNumber - 1) * pageSize;
        return await _dbSet
            .Where(da => da.UserId == userId)
            .Skip(skipAmount)
            .Take(pageSize)
            .ToListAsync();
    }
}
