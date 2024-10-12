using AutoMapper;
using AutomationService.Application.Common.Services.Interfaces;
using AutomationService.Application.Features.Routine.Commands;
using AutomationService.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace AutomationService.Application.Features.Routine.Handlers.Commands;

public class UpdateRoutineCommandHandler(
    IRoutineRepository routineRepository,
    IMapper mapper,
    IRoutineSchedulerService schedulerFactory,
    IValidator<UpdateRoutineCommand> validator
) : IRequestHandler<UpdateRoutineCommand>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IRoutineSchedulerService _schedulerFactory = schedulerFactory;
    private readonly IValidator<UpdateRoutineCommand> _validator = validator;

    public async Task<Unit> Handle(
        UpdateRoutineCommand request,
        CancellationToken cancellationToken
    )
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken);
        var routine =
            await _routineRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException("Rotina não encontrada.");

        if (
            request.ExecutionTime == TimeSpan.Zero
            && request.ActionType == routine.ActionType
            && request.Brightness == routine.Brightness
        )
            throw new InvalidOperationException("Nenhuma alteração foi feita.");
        else if (request.ExecutionTime == TimeSpan.Zero)
            request.ExecutionTime = routine.ExecutionTime;

        _mapper.Map(request, routine);

        await _routineRepository.UpdateAsync(routine, cancellationToken);
        await _schedulerFactory.UpdateRoutineAsync(routine);

        return Unit.Value;
    }
}
