using AutomationService.Application.Features.Routine.Command;
using MediatR;
using Quartz;

namespace AutomationService.Application.Common.Jobs;

public class RoutineJob(IMediator mediator) : IJob
{
    private readonly IMediator _mediator = mediator;

    public async Task Execute(IJobExecutionContext context)
    {
        // Obter o ID da rotina agendada dos dados do contexto
        var routineId = context.MergedJobDataMap.GetString("RoutineId");

        if (Guid.TryParse(routineId, out Guid parsedRoutineId))
        {
            // Executar a rotina usando o Mediator para chamar o handler apropriado
            await _mediator.Send(new ExecuteRoutineCommand { RoutineId = parsedRoutineId });
        }
    }
}
