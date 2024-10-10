using AutomationService.Application.Common.Services;
using AutomationService.Application.Features.Routine.Command;
using AutomationService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Entity = AutomationService.Domain.Entities;

namespace AutomationService.Application.Features.Routine.Handlers;

public class CreateRoutineCommandHandler(
    IRoutineRepository routineRepository,
    RoutineSchedulerService schedulerService,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<CreateRoutineCommand, Guid>
{
    private readonly IRoutineRepository _routineRepository = routineRepository;
    private readonly RoutineSchedulerService _schedulerService = schedulerService;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Guid> Handle(
        CreateRoutineCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();
        var token = _httpContextAccessor.HttpContext?.Items["JwtToken"]?.ToString();

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Falta o ID do usuário.");
        if (string.IsNullOrEmpty(token))
            throw new UnauthorizedAccessException("Falta o token JWT.");

        var routine = new Entity.Routine
        {
            UserId = Guid.Parse(userId),
            Name = request.Name,
            ExecutionTime = request.ExecutionTime,
            ActionType = request.ActionType,
            TargetId = request.TargetId,
            Brightness = request.Brightness,
            ActionTarget = request.ActionTarget,
            JwtToken = token,
        };

        await _routineRepository.CreateAsync(routine, cancellationToken);
        await _schedulerService.ScheduleRoutineAsync(routine);

        return routine.Id;
    }
}
