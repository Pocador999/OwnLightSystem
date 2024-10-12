using AutoMapper;
using AutomationService.Application.Features.Routine.Commands;
using AutomationService.Domain.Interfaces;
using MediatR;

namespace AutomationService.Application.Features.Routine.Handlers.Commands;

public class UpdateRoutineNameCommandHandler(IRoutineRepository routineRepository, IMapper mapper)
    : IRequestHandler<UpdateRoutineNameCommand>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Unit> Handle(
        UpdateRoutineNameCommand request,
        CancellationToken cancellationToken
    )
    {
        var routine =
            await _routineRepository.GetByIdAsync(request.Id)
            ?? throw new KeyNotFoundException("Rotina não encontrada.");

        if (request.Name == routine.Name)
            throw new InvalidOperationException("Nenhum nome foi alterado.");
        else if (request.Name == "string")
            throw new InvalidOperationException("Nome inválido.");

        _mapper.Map(request, routine);

        await _routineRepository.UpdateAsync(routine, cancellationToken);

        return Unit.Value;
    }
}
