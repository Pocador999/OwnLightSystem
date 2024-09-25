using AutoMapper;
using DeviceService.Application.DTOs;
using DeviceService.Application.Features.Device.Commands;
using DeviceService.Application.Features.DeviceType.Commands;
using DeviceService.Domain.Entities;

namespace DeviceService.Application.Common.Mappings;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        // Mapping for Device Entity
        CreateMap<Device, DeviceResponseDTO>().ReverseMap();
        CreateMap<Device, CreateDeviceCommand>();
        CreateMap<CreateDeviceCommand, Device>()
            .ForMember(dest => dest.DeviceType, opt => opt.Ignore())
            .ForMember(dest => dest.DeviceActions, opt => opt.Ignore());
        CreateMap<UpdateDeviceCommand, Device>();

        // Mapping for DeviceType Entity
        CreateMap<DeviceType, DeviceTypeResponseDTO>().ReverseMap();
        CreateMap<DeviceType, CreateDeviceTypeCommand>();
        CreateMap<CreateDeviceTypeCommand, DeviceType>()
            .ForMember(dest => dest.Devices, opt => opt.Ignore());
    }
}
