using AutomationService.Application.Common.Services.Interfaces;
using AutomationService.Domain.Entities;
using AutomationService.Domain.Enums;
using AutomationService.Domain.Interfaces;

namespace AutomationService.Application.Common.Services;

public class RoutineExecutionLogService(
    IRoutineExecutionLogRepository routineExecutionLogRepository
) : IRoutineExecutionLogService
{
    private readonly IRoutineExecutionLogRepository _routineExecutionLogRepository =
        routineExecutionLogRepository;

    public async Task LogAsync(
        Guid userId,
        Guid routineId,
        ActionTarget actionTarget,
        Guid targetId,
        RoutineActionType actionType,
        ActionStatus actionStatus,
        string errorMessage,
        CancellationToken cancellationToken = default
    )
    {
        var log = new RoutineExecutionLog
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            RoutineId = routineId,
            ActionTarget = actionTarget,
            TargetId = targetId,
            ActionType = actionType,
            ActionStatus = actionStatus,
            ExecutedAt = DateTime.UtcNow,
            ErrorMessage = errorMessage,
        };

        await _routineExecutionLogRepository.CreateAsync(log, cancellationToken);
    }
}
