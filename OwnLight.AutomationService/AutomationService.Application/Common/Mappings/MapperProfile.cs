using AutoMapper;
using AutomationService.Application.Contracts.DTOs;
using AutomationService.Application.Features.Routine.Commands;
using AutomationService.Domain.Entities;

namespace AutomationService.Application.Common.Mappings;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Routine, CreateRoutineCommand>().ReverseMap();
        CreateMap<Routine, UpdateRoutineNameCommand>().ReverseMap();
        CreateMap<Routine, UpdateRoutineCommand>().ReverseMap();

        CreateMap<Routine, RoutineResponseDTO>()
            .ForMember(
                dest => dest.ActionType,
                opt => opt.MapFrom(src => src.ActionType.ToString())
            )
            .ForMember(
                dest => dest.ActionTarget,
                opt => opt.MapFrom(src => src.ActionTarget.ToString())
            );

        CreateMap<RoutineExecutionLog, RoutineLogDTO>()
            .ForMember(
                dest => dest.ActionType,
                opt => opt.MapFrom(src => src.ActionType.ToString())
            )
            .ForMember(
                dest => dest.ActionTarget,
                opt => opt.MapFrom(src => src.ActionTarget.ToString())
            )
            .ForMember(
                dest => dest.ActionStatus,
                opt => opt.MapFrom(src => src.ActionStatus.ToString())
            );
    }
}
