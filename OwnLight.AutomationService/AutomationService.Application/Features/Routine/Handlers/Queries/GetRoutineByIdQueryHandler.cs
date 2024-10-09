using AutoMapper;
using AutomationService.Application.DTOs;
using AutomationService.Application.Features.Routine.Queries;
using AutomationService.Domain.Interfaces;
using MediatR;

namespace AutomationService.Application.Features.Routine.Handlers.Queries;

public class GetRoutineByIdQueryHandler(IRoutineRepository routineRepository, IMapper mapper)
    : IRequestHandler<GetRoutineByIdQuery, RoutineResponseDTO>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<RoutineResponseDTO> Handle(
        GetRoutineByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var routine =
            await _routineRepository.GetByIdAsync(request.Id)
            ?? throw new Exception("Rotina não encontrada");

        return _mapper.Map<RoutineResponseDTO>(routine);
    }
}
