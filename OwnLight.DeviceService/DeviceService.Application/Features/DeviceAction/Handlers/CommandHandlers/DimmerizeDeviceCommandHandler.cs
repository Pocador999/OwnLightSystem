using DeviceService.Application.Features.DeviceAction.Commands;
using DeviceService.Domain.Enums;
using DeviceService.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Entity = DeviceService.Domain.Entities;

namespace DeviceService.Application.Features.DeviceAction.Handlers.CommandHandlers;

public class DimmerizeDeviceCommandHandler(
    IDeviceRepository deviceRepository,
    IDeviceActionRepository deviceActionRepository,
    IHttpContextAccessor httpContextAccessor,
    IValidator<DimmerizeDeviceCommand> validator
) : IRequestHandler<DimmerizeDeviceCommand>
{
    private readonly IDeviceRepository _deviceRepository = deviceRepository;
    private readonly IDeviceActionRepository _deviceActionRepository = deviceActionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IValidator<DimmerizeDeviceCommand> _validator = validator;

    public async Task<Unit> Handle(
        DimmerizeDeviceCommand request,
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

        if (device.IsDimmable == false)
            throw new InvalidOperationException("Este dispositivo não suporta ajuste de brilho.");

        if (request.Brightness < 0 || request.Brightness > 100)
            throw new ArgumentOutOfRangeException("Brightness deve estar entre 0 e 100.");

        try
        {
            if (request.Brightness > 0 && device.Status == DeviceStatus.Off)
            {
                device = await _deviceRepository.ControlDeviceAsync(
                    request.DeviceId,
                    DeviceStatus.On
                );
            }
            else if (request.Brightness == 0 && device.Status == DeviceStatus.On)
            {
                device = await _deviceRepository.ControlDeviceAsync(
                    request.DeviceId,
                    DeviceStatus.Off
                );
            }

            device.Brightness = request.Brightness;
            await _deviceRepository.UpdateAsync(device);

            var deviceAction = new Entity.DeviceAction
            {
                DeviceId = device.Id,
                UserId = Guid.Parse(userId),
                Action = DeviceActions.Dim,
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
                Action = DeviceActions.Dim,
                Status = ActionStatus.Failed,
            };

            await _deviceActionRepository.CreateAsync(deviceAction);
            throw;
        }

        return Unit.Value;
    }
}
