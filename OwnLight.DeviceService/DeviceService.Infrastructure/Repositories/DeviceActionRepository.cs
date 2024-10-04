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
}
