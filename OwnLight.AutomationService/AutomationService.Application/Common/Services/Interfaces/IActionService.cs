using AutomationService.Domain.Enums;

namespace AutomationService.Application.Common.Services.Interfaces;

public interface IActionService
{
    public Task<bool> ControlDeviceAsync(
        Guid deviceId,
        RoutineActionType status,
        CancellationToken cancellationToken = default
    );
}
