using AutoMapper;
using AutomationService.Application.DTOs;
using AutomationService.Domain.Entities;

namespace AutomationService.Application.Common.Mappings;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Routine, RoutineResponseDTO>()
            .ForMember(
                dest => dest.ActionTarget,
                opt => opt.MapFrom(src => src.ActionTarget.ToString())
            )
            .ForMember(
                dest => dest.ActionType,
                opt => opt.MapFrom(src => src.ActionType.ToString())
            );

        CreateMap<RoutineExecutionLog, RoutineLogResponseDTO>()
            .ForMember(
                dest => dest.ActionStatus,
                opt => opt.MapFrom(src => src.ActionStatus.ToString())
            )
            .ForMember(
                dest => dest.ActionTarget,
                opt => opt.MapFrom(src => src.ActionTarget.ToString())
            )
            .ForMember(
                dest => dest.ActionType,
                opt => opt.MapFrom(src => src.ActionType.ToString())
            );
    }
}
