using AutoMapper;
using AutomationService.Application.Contracts.DTOs;
using AutomationService.Application.Features.Routine.Queries;
using AutomationService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AutomationService.Application.Features.Routine.Handlers.Queries;

public class GetLogsByUserIdQueryHandler
    : IRequestHandler<GetLogsByUserIdQuery, PaginatedResultDTO<RoutineLogDTO>>
{
    private readonly IRoutineExecutionLogRepository _logRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetLogsByUserIdQueryHandler(
        IRoutineExecutionLogRepository routineRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _logRepository = routineRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<PaginatedResultDTO<RoutineLogDTO>> Handle(
        GetLogsByUserIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Falha ao obter o ID do usuário.");

        var logs = await _logRepository.GetByUserId(
            Guid.Parse(userId),
            request.PageNumber,
            request.PageSize,
            cancellationToken
        );
        var totalCount = await _logRepository.CountAsync(cancellationToken);
        var logsDTO = _mapper.Map<IEnumerable<RoutineLogDTO>>(logs);

        return new PaginatedResultDTO<RoutineLogDTO>(
            totalCount,
            request.PageNumber,
            request.PageSize,
            logsDTO
        );
    }
}
