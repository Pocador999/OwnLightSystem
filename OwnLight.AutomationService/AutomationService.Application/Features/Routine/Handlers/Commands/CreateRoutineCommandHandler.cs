using System.Security.Claims;
using AutomationService.Application.Features.Routine.Commands;
using AutomationService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Entity = AutomationService.Domain.Entities;

namespace AutomationService.Application.Features.Routine.Handlers.Commands;

public class CreateRoutineCommandHandler : IRequestHandler<CreateRoutineCommand>
{
    private readonly IRoutineRepository _routineRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateRoutineCommandHandler(
        IRoutineRepository routineRepository,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _routineRepository = routineRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Unit> Handle(
        CreateRoutineCommand request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        var routine = new Entity.Routine
        {
            Id = Guid.Parse(userId),
            UserId = request.UserId,
            Name = request.Name,
            ExecutionTime = request.ExecutionTime,
            ActionTarget = request.ActionTarget,
            TargetId = request.TargetId,
            ActionType = request.ActionType,
            Brightness = request.Brightness,
        };

        await _routineRepository.CreateAsync(routine, cancellationToken);
        return Unit.Value;
    }
}
