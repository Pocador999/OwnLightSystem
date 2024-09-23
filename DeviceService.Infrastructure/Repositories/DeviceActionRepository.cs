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

    public Task<IEnumerable<DeviceAction>> GetUserDeviceActionsAsync(
        Guid userId,
        int page,
        int pageSize
    )
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DeviceAction>> GetUserDeviceActionsByDeviceAsync(
        Guid userId,
        Guid deviceId
    )
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DeviceAction>> GetUserDeviceActionsByGroupAsync(
        Guid userId,
        Guid groupId
    )
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DeviceAction>> GetUserDeviceActionsByRoomAsync(Guid userId, Guid roomId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DeviceAction>> GetUserDeviceActionsByTypeAsync(
        Guid userId,
        Guid deviceType
    )
    {
        throw new NotImplementedException();
    }
}
