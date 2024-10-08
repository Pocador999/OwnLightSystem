using AutoMapper;
using DeviceService.Application.Features.Device.Commands;
using DeviceService.Domain.Enums;
using DeviceService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Entity = DeviceService.Domain.Entities;

namespace DeviceService.Application.Features.Device.Handlers.CommandHandlers;

public class CreateDeviceCommandHandler(
    IDeviceRepository deviceRepository,
    IDeviceTypeRepository deviceTypeRepository,
    IHttpContextAccessor httpContextAccessor,
    IMapper mapper
) : IRequestHandler<CreateDeviceCommand, Guid>
{
    private readonly IDeviceRepository _deviceRepository = deviceRepository;
    private readonly IDeviceTypeRepository _deviceTypeRepository = deviceTypeRepository;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IMapper _mapper = mapper;

    public async Task<Guid> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();

        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        var deviceType =
            await _deviceTypeRepository.GetDeviceTypeByNameAsync(request.DeviceType)
            ?? throw new ArgumentException($"Device type '{request.DeviceType}' not found.");

        var device = _mapper.Map<Entity.Device>(request);
        device.DeviceType = deviceType;
        device.UserId = Guid.Parse(userId);
        device.Status = DeviceStatus.Off;

        await _deviceRepository.CreateAsync(device);

        return device.Id;
    }
}
