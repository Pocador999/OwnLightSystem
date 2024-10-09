using AutomationService.Domain.Enums;

namespace AutomationService.Application.Common.Services.Interfaces;

public interface IRoutineExecutionLogService
{
    public Task LogAsync(
        Guid userId,
        Guid routineId,
        ActionTarget actionTarget,
        Guid targetId,
        RoutineActionType actionType,
        ActionStatus actionStatus,
        string errorMessage,
        CancellationToken cancellationToken = default
    );
}
