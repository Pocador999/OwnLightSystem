using DeviceService.Application.Features.DeviceAction.Commands;
using DeviceService.Domain.Enums;
using DeviceService.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Entity = DeviceService.Domain.Entities;

namespace DeviceService.Application.Features.DeviceAction.Handlers.CommandHandlers;

public class ControlDeviceCommandHandler(
    IDeviceRepository deviceRepository,
    IDeviceActionRepository deviceActionRepository,
    IHttpContextAccessor httpContextAccessor,
    IValidator<ControlDeviceCommand> validator
) : IRequestHandler<ControlDeviceCommand>
{
    private readonly IDeviceRepository _deviceRepository = deviceRepository;
    private readonly IDeviceActionRepository _deviceActionRepository = deviceActionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IValidator<ControlDeviceCommand> _validator = validator;

    public async Task<Unit> Handle(
        ControlDeviceCommand request,
        CancellationToken cancellationToken
    )
    {
        await _validator.ValidateAndThrowAsync(request, cancellationToken: cancellationToken);

        var userId = _httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        var device =
            await _deviceRepository.GetByIdAsync(request.DeviceId)
            ?? throw new KeyNotFoundException(
                $"Dispositivo de id {request.DeviceId} não encontrado."
            );

        if (device.UserId.ToString() != userId)
            throw new UnauthorizedAccessException(
                $"O dispositivo de id {request.DeviceId} não pertence ao usuário."
            );

        try
        {
            if (request.Status == DeviceStatus.On)
            {
                device = await _deviceRepository.ControlDeviceAsync(
                    request.DeviceId,
                    DeviceStatus.On
                );
                if (device.IsDimmable == true)
                    device.Brightness = 100;
            }
            else if (request.Status == DeviceStatus.Off)
            {
                device = await _deviceRepository.ControlDeviceAsync(
                    request.DeviceId,
                    DeviceStatus.Off
                );
                if (device.IsDimmable == true)
                    device.Brightness = 0;
            }

            await _deviceRepository.UpdateAsync(device);

            var deviceAction = new Entity.DeviceAction
            {
                DeviceId = device.Id,
                UserId = Guid.Parse(userId),
                Action =
                    request.Status == DeviceStatus.On
                        ? DeviceActions.TurnOn
                        : DeviceActions.TurnOff,
                Status = ActionStatus.Success,
            };

            await _deviceActionRepository.CreateAsync(deviceAction);
        }
        catch (Exception)
        {
            var deviceAction = new Entity.DeviceAction
            {
                DeviceId = device.Id,
                UserId = Guid.Parse(userId),
                Action =
                    request.Status == DeviceStatus.On
                        ? DeviceActions.TurnOn
                        : DeviceActions.TurnOff,
                Status = ActionStatus.Failed,
            };

            await _deviceActionRepository.CreateAsync(deviceAction);
            throw;
        }

        return Unit.Value;
    }
}
