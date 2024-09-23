using DeviceService.Domain.Entities;
using DeviceService.Domain.Interfaces;
using DeviceService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DeviceService.Infrastructure.Repositories;

public class DeviceTypeRepository(DataContext dataContext)
    : Repository<DeviceType>(dataContext),
        IDeviceTypeRepository
{
    private readonly DbSet<DeviceType> _dbSet = dataContext.Set<DeviceType>();

    public Task<DeviceType?> GetDeviceTypeByNameAsync(string typeName) =>
        _dbSet.FirstOrDefaultAsync(x => x.TypeName == typeName);

    public Task<IEnumerable<DeviceType>> GetUserDeviceTypesAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DeviceType>> GetUserDeviceTypesByGroupAsync(Guid userId, Guid groupId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DeviceType>> GetUserDeviceTypesByRoomAsync(Guid userId, Guid roomId)
    {
        throw new NotImplementedException();
    }
}
