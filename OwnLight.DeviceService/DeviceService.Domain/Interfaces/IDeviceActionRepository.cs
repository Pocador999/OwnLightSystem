using DeviceService.Domain.Entities;
using DeviceService.Domain.Enums;

namespace DeviceService.Domain.Interfaces;

public interface IDeviceActionRepository : IRepository<DeviceAction>
{
    Task<DeviceAction> AddDeviceActionAsync(DeviceAction deviceAction);
    Task<IEnumerable<DeviceAction>> GetUserActionsAsync(Guid userId, int pageNumber, int pageSize);
    Task<IEnumerable<DeviceAction>> GetActionsByDeviceIdAsync(
        Guid deviceId,
        int pageNumber,
        int pageSize
    );
    Task<IEnumerable<DeviceAction>> GetUserActionsByStatusAsync(
        Guid userId,
        ActionStatus actionStatus,
        int pageNumber,
        int pageSize
    );
    Task<IEnumerable<DeviceAction>> GetUserActionsByTypeAsync(
        Guid userId,
        DeviceActions actionType,
        int pageNumber,
        int pageSize
    );
}
