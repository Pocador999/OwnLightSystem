using AutomationService.Application.Common.Services.Interfaces;
using AutomationService.Application.Features.Routine.Command;
using AutomationService.Domain.Entities;
using AutomationService.Domain.Enums;
using AutomationService.Domain.Interfaces;
using MediatR;

namespace AutomationService.Application.Features.Routine.Handlers;

public class ExecuteRoutineCommandHandler(
    IRoutineRepository routineRepository,
    IRoutineExecutionLogRepository routineExecutionLogRepository,
    IDeviceServiceClient deviceServiceClient
) : IRequestHandler<ExecuteRoutineCommand>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;
    private readonly IRoutineExecutionLogRepository _routineExecutionLogRepository =
        routineExecutionLogRepository;
    private readonly IDeviceServiceClient _deviceServiceClient = deviceServiceClient;

    public async Task<Unit> Handle(
        ExecuteRoutineCommand request,
        CancellationToken cancellationToken
    )
    {
        var routine =
            await _routineRepository.GetByIdAsync(request.RoutineId)
            ?? throw new KeyNotFoundException("Rotina não encontrada.");

        var result = await _deviceServiceClient.ExecuteActionAsync(routine);

        var executionLog = new RoutineExecutionLog
        {
            Id = Guid.NewGuid(),
            RoutineId = routine.Id,
            UserId = routine.UserId,
            TargetId = routine.TargetId ?? Guid.Empty,
            ActionType = routine.ActionType,
            ActionStatus = result.IsSuccess ? ActionStatus.Success : ActionStatus.Failed,
            ExecutedAt = DateTime.UtcNow,
            ErrorMessage = result.ErrorMessage,
        };

        await _routineExecutionLogRepository.CreateAsync(executionLog, cancellationToken);
        return Unit.Value;
    }
}
