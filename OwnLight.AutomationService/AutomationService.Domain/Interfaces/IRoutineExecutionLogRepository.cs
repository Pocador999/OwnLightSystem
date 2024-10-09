using AutomationService.Domain.Entities;
using AutomationService.Domain.Enums;

namespace AutomationService.Domain.Interfaces;

public interface IRoutineExecutionLogRepository : IRepository<RoutineExecutionLog>
{
    Task<IEnumerable<RoutineExecutionLog>> GetByRoutineId(
        Guid routineId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<RoutineExecutionLog>> GetByUserId(
        Guid userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<RoutineExecutionLog>> GetByDeviceId(
        Guid deviceId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<RoutineExecutionLog>> GetByActionTarget(
        ActionTarget actionTarget,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<RoutineExecutionLog>> GetByRoutineActionType(
        RoutineActionType actionType,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );
    Task<IEnumerable<RoutineExecutionLog>> GetByActionStatus(
        ActionStatus actionStatus,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default
    );
}
