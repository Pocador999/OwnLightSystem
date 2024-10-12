using AutoMapper;
using AutomationService.Application.Features.Routine.Commands;
using AutomationService.Domain.Entities;

namespace AutomationService.Application.Common.Mappings;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Routine, CreateRoutineCommand>().ReverseMap();
        CreateMap<Routine, UpdateRoutineCommand>().ReverseMap();
    }
}
