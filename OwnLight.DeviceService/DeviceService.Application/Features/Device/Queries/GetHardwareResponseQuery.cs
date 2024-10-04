using DeviceService.Application.DTOs;
using MediatR;

namespace DeviceService.Application.Features.Device.Queries;

public class GetHardwareResponseQuery(Guid[] deviceIds, int pageNumber, int pageSize)
    : IRequest<PaginatedResultDTO<HardwareResponseDTO>>
{
    public Guid[] DeviceIds { get; } = deviceIds;
    public int PageNumber { get; } = pageNumber;
    public int PageSize { get; } = pageSize;
}
