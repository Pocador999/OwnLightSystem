using DeviceService.Application.Features.DeviceAction.Commands;
using DeviceService.Domain.Enums;
using DeviceService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Entity = DeviceService.Domain.Entities;

namespace DeviceService.Application.Features.DeviceAction.Handlers;

public class ControlDeviceCommandHandler(
    IDeviceRepository deviceRepository,
    IDeviceActionRepository deviceActionRepository,
    IHttpContextAccessor httpContextAccessor
) : IRequestHandler<ControlDeviceCommand>
{
    private readonly IDeviceRepository _deviceRepository = deviceRepository;
    private readonly IDeviceActionRepository _deviceActionRepository = deviceActionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<Unit> Handle(
        ControlDeviceCommand request,
        CancellationToken cancellationToken
    )
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
            // Ajusta o brilho de acordo com o estado
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

            // Cria um registro de ação no banco de dados
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
            // Caso ocorra algum erro, cria um registro com status "Failed"
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
