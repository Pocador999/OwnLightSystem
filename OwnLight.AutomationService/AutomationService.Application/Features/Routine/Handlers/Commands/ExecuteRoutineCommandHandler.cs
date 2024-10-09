using AutomationService.Application.Common.Services.Interfaces;
using AutomationService.Application.Features.Routine.Commands;
using AutomationService.Domain.Enums;
using AutomationService.Domain.Interfaces;
using MediatR;

namespace AutomationService.Application.Features.Routine.Handlers.Commands;

public class ExecuteRoutineCommandHandler : IRequestHandler<ExecuteRoutineCommand>
{
    private readonly IRoutineRepository _routineRepository;
    private readonly IActionService _actionService;
    private readonly IRoutineExecutionLogService _routineExecutionLogService;

    public ExecuteRoutineCommandHandler(
        IRoutineRepository routineRepository,
        IActionService actionService,
        IRoutineExecutionLogService routineExecutionLogService
    )
    {
        _routineRepository = routineRepository;
        _actionService = actionService;
        _routineExecutionLogService = routineExecutionLogService;
    }

    public async Task<Unit> Handle(
        ExecuteRoutineCommand request,
        CancellationToken cancellationToken
    )
    {
        var routinesToExecute = await _routineRepository.GetRoutinesToExecuteAsync(
            request.CurrentTime,
            cancellationToken
        );

        foreach (var routine in routinesToExecute)
        {
            try
            {
                var result = await _actionService.ControlDeviceAsync(
                    routine.TargetId,
                    routine.ActionType,
                    cancellationToken
                );

                var actionStatus = result ? ActionStatus.Success : ActionStatus.Failed;
                await _routineExecutionLogService.LogAsync(
                    routine.UserId,
                    routine.Id,
                    routine.ActionTarget,
                    routine.TargetId,
                    routine.ActionType,
                    actionStatus,
                    errorMessage: string.Empty,
                    cancellationToken
                );
            }
            catch (Exception ex)
            {
                await _routineExecutionLogService.LogAsync(
                    routine.UserId,
                    routine.Id,
                    routine.ActionTarget,
                    routine.TargetId,
                    routine.ActionType,
                    ActionStatus.Failed,
                    ex.Message,
                    cancellationToken
                );
            }
        }

        return Unit.Value;
    }
}
