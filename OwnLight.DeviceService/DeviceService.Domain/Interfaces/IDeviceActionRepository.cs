using DeviceService.Domain.Entities;

namespace DeviceService.Domain.Interfaces;

public interface IDeviceActionRepository : IRepository<DeviceAction>
{
    Task<DeviceAction> AddDeviceActionAsync(DeviceAction deviceAction);
    Task<IEnumerable<DeviceAction>> GetUserActionsAsync(Guid userId, int pageNumber, int pageSize);
}
