using AutoMapper;
using AutomationService.Application.Common.Services.Interfaces;
using AutomationService.Application.Features.Routine.Commands;
using AutomationService.Domain.Interfaces;
using MediatR;

namespace AutomationService.Application.Features.Routine.Handlers;

public class UpdateRoutineCommandHandler(
    IRoutineRepository routineRepository,
    IRoutineSchedulerService schedulerFactory,
    IMapper mapper
) : IRequestHandler<UpdateRoutineCommand>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;
    private readonly IRoutineSchedulerService _schedulerFactory = schedulerFactory;
    private readonly IMapper _mapper = mapper;

    public async Task<Unit> Handle(
        UpdateRoutineCommand request,
        CancellationToken cancellationToken
    )
    {
        var routine =
            await _routineRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException("Rotina não encontrada.");

        _mapper.Map(request, routine);

        await _routineRepository.UpdateAsync(routine, cancellationToken);
        await _schedulerFactory.UpdateRoutineAsync(routine);

        return Unit.Value;
    }
}
