using AutoMapper;
using UserService.Application.DTOs;
using UserService.Application.Features.User.Commands;
using UserService.Domain.Entities;

namespace UserService.Application.Mappings;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserResponseDTO>().ReverseMap();
        CreateMap<User, CreateCommand>().ReverseMap();
        CreateMap<User, UpdateCommand>().ReverseMap();
    }
}
