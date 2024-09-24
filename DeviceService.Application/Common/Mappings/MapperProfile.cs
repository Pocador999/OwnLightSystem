using AutoMapper;
using DeviceService.Application.DTOs;
using DeviceService.Application.Features.Device.Commands;
using DeviceService.Domain.Entities;

namespace DeviceService.Application.Common.Mappings;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Device, DeviceReponseDTO>().ReverseMap();
        CreateMap<Device, CreateDeviceCommand>();
        CreateMap<CreateDeviceCommand, Device>()
            .ForMember(dest => dest.DeviceType, opt => opt.Ignore())
            .ForMember(dest => dest.DeviceActions, opt => opt.Ignore());
        CreateMap<UpdateDeviceCommand, Device>();
    }
}
