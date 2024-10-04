using AutoMapper;
using DeviceService.Application.DTOs;
using DeviceService.Application.Features.Device.Queries;
using DeviceService.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DeviceService.Application.Features.Device.Handlers.QueryHandlers;

public class GetUserDevicesByGroupIdQueryHandler
    : IRequestHandler<GetUserDevicesByGroupIdQuery, PaginatedResultDTO<UserResponseDTO>>
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetUserDevicesByGroupIdQueryHandler(
        IDeviceRepository deviceRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _deviceRepository = deviceRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<PaginatedResultDTO<UserResponseDTO>> Handle(
        GetUserDevicesByGroupIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var userId = _httpContextAccessor.HttpContext?.Items["UserId"]?.ToString();
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("Usuário não autenticado.");

        if (request.GroupId == Guid.Empty)
            throw new ArgumentException("GroupId Inválido ou não encontrado.");

        var devices = await _deviceRepository.GetUserDevicesByGroupIdAsync(
            Guid.Parse(userId),
            request.GroupId,
            request.PageNumber,
            request.PageSize
        );
        var devicesDTO = _mapper.Map<IEnumerable<UserResponseDTO>>(devices);

        return new PaginatedResultDTO<UserResponseDTO>(
            devicesDTO.Count(),
            request.PageNumber,
            request.PageSize,
            devicesDTO
        );
    }
}
