using DeviceService.Application.Features.DeviceAction.Commands;
using DeviceService.Domain.Enums;
using DeviceService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Entity = DeviceService.Domain.Entities;

namespace DeviceService.Application.Features.DeviceAction.Handlers;

public class SwitchDeviceCommandHandler(
    IDeviceRepository deviceRepository,
    IDeviceActionRepository deviceActionRepository,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<SwitchDeviceCommand>
{
    private readonly IDeviceRepository _deviceRepository = deviceRepository;
    private readonly IDeviceActionRepository _deviceActionRepository = deviceActionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Unit> Handle(SwitchDeviceCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        var device =
            await _deviceRepository.GetByIdAsync(request.DeviceId)
            ?? throw new KeyNotFoundException(
                $"Dispositivo de id {request.DeviceId} não encontrado."
            );

        // Verifica se o dispositivo pertence ao usuário
        if (device.UserId.ToString() != userId)
            throw new UnauthorizedAccessException(
                $"O dispositivo de id {request.DeviceId} não pertence ao usuário."
            );

        try
        {
            // Alterna o estado do dispositivo
            if (device.Status == DeviceStatus.Off)
            {
                device = await _deviceRepository.ControlDeviceAsync(
                    request.DeviceId,
                    DeviceStatus.On
                );
                if (device.IsDimmable == true)
                    device.Brightness = 100;
            }
            else
            {
                device = await _deviceRepository.ControlDeviceAsync(
                    request.DeviceId,
                    DeviceStatus.Off
                );
                if (device.IsDimmable == true)
                    device.Brightness = 0;
            }

            await _deviceRepository.UpdateAsync(device);

            // Cria um registro de ação no banco de dados
            var deviceAction = new Entity.DeviceAction
            {
                DeviceId = device.Id,
                UserId = Guid.Parse(userId),
                Action =
                    device.Status == DeviceStatus.On ? DeviceActions.TurnOn : DeviceActions.TurnOff,
                Status = ActionStatus.Success,
            };

            await _deviceActionRepository.AddDeviceActionAsync(deviceAction);
        }
        catch (Exception)
        {
            // Caso ocorra algum erro, cria um registro com status "Failed"
            var deviceAction = new Entity.DeviceAction
            {
                DeviceId = device.Id,
                UserId = Guid.Parse(userId),
                Action =
                    device.Status == DeviceStatus.On ? DeviceActions.TurnOn : DeviceActions.TurnOff,
                Status = ActionStatus.Failed,
            };

            await _deviceActionRepository.AddDeviceActionAsync(deviceAction);
            throw;
        }

        return Unit.Value;
    }
}
