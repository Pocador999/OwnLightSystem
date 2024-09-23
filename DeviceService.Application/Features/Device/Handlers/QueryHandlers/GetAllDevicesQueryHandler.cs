using AutoMapper;
using DeviceService.Application.DTOs;
using DeviceService.Application.Features.Device.Queries;
using DeviceService.Domain.Interfaces;
using MediatR;

namespace DeviceService.Application.Features.Device.Handlers.QueryHandlers;

public class GetAllDevicesQueryHandler : IRequestHandler<GetAllDevicesQuery, PaginatedResultDTO>
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IMapper _mapper;

    public GetAllDevicesQueryHandler(IDeviceRepository deviceRepository, IMapper mapper)
    {
        _deviceRepository = deviceRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResultDTO> Handle(
        GetAllDevicesQuery request,
        CancellationToken cancellationToken
    )
    {
        var devices = await _deviceRepository.GetAllAsync(request.Page, request.PageSize);
        var devicesResponse = _mapper.Map<IEnumerable<DeviceReponseDTO>>(devices);

        return new PaginatedResultDTO(
            devicesResponse.Count(),
            request.Page,
            request.PageSize,
            devicesResponse
        );
    }
}
