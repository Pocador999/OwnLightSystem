using AutoMapper;
using DeviceService.Application.DTOs;
using DeviceService.Application.Features.Device.Queries;
using DeviceService.Domain.Interfaces;
using MediatR;

namespace DeviceService.Application.Features.Device.Handlers.QueryHandlers;

public class GetDeviceByIdQueryHandler(IDeviceRepository deviceRepository, IMapper mapper)
    : IRequestHandler<GetDeviceByIdQuery, DeviceReponseDTO>
{
    private readonly IDeviceRepository _deviceRepository = deviceRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<DeviceReponseDTO> Handle(
        GetDeviceByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var device = await _deviceRepository.GetByIdAsync(request.Id);

        return _mapper.Map<DeviceReponseDTO>(device);
    }
}
