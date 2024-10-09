using AutomationService.Application.Features.Routine.Commands;
using MediatR;
using Quartz;

namespace AutomationService.Application.Common.Jobs;

public class RoutineJob : IJob
{
    private readonly IMediator _mediator;

    public RoutineJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var currentTime = DateTime.UtcNow.TimeOfDay;

        var command = new ExecuteRoutineCommand { CurrentTime = currentTime };
        await _mediator.Send(command);
    }
}
