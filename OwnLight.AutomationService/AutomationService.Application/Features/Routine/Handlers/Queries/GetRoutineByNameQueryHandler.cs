using AutoMapper;
using AutomationService.Application.DTOs;
using AutomationService.Application.Features.Routine.Queries;
using AutomationService.Domain.Interfaces;
using MediatR;

namespace AutomationService.Application.Features.Routine.Handlers.Queries;

public class GetRoutineByNameQueryHandler(IRoutineRepository routineRepository, IMapper mapper)
    : IRequestHandler<GetRoutineByNameQuery, RoutineResponseDTO>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<RoutineResponseDTO> Handle(
        GetRoutineByNameQuery request,
        CancellationToken cancellationToken
    )
    {
        var routine =
            await _routineRepository.GetRoutineByNameAsync(request.Name, cancellationToken)
            ?? throw new Exception("Routine not found");

        return _mapper.Map<RoutineResponseDTO>(routine);
    }
}
