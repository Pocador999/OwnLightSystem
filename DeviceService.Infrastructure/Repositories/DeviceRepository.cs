using DeviceService.Domain.Entities;
using DeviceService.Domain.Interfaces;
using DeviceService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeviceService.Infrastructure.Repositories;

public class DeviceRepository(DataContext dataContext)
    : Repository<Device>(dataContext),
        IDeviceRepository
{
    private readonly DbSet<Device> _dbSet = dataContext.Set<Device>();

    public Task<IEnumerable<Device>> DeleteUserDevicesAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Device>> DeleteUserDevicesByGroupAsync(Guid userId, Guid groupId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Device>> DeleteUserDevicesByRoomAsync(Guid userId, Guid roomId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Device>> DeleteUserDevicesByTypeAsync(Guid userId, Guid deviceType)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Device>> GetUserDevicesAsync(Guid userId, int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Device>> GetUserDevicesByGroupAsync(Guid userId, Guid groupId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Device>> GetUserDevicesByRoomAsync(Guid userId, Guid roomId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Device>> GetUserDevicesByTypeAsync(Guid userId, Guid deviceType)
    {
        throw new NotImplementedException();
    }
}
