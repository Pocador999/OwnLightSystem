using AutomationService.Domain.Enums;

namespace AutomationService.Application.Common.Services.Interfaces;

public interface IActionService
{
    public Task ControlDeviceAsync(
        Guid deviceId,
        DeviceStatus status,
        RecurrencePattern recurrencePattern,
        RoutineActionType actionType,
        CancellationToken cancellationToken = default
    );

    public Task ControlRoomAsync(
        Guid roomId,
        DeviceStatus status,
        RecurrencePattern recurrencePattern,
        RoutineActionType actionType,
        CancellationToken cancellationToken = default
    );

    public Task ControlGroupAsync(
        Guid groupId,
        DeviceStatus status,
        RecurrencePattern recurrencePattern,
        RoutineActionType actionType,
        CancellationToken cancellationToken = default
    );

    public Task ControlHomeAsync(
        DeviceStatus status,
        RecurrencePattern recurrencePattern,
        RoutineActionType actionType,
        CancellationToken cancellationToken = default
    );
}
