using AutoMapper;
using UserService.Application.DTOs;
using UserService.Application.Features.User.Commands;
using UserService.Application.Features.User.Commands.Update;
using UserService.Domain.Entities;

namespace UserService.Application.Common.Mappings;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User, UserResponseDTO>().ReverseMap();
        CreateMap<User, AdminResponseDTO>().ReverseMap();
        CreateMap<User, CreateCommand>().ReverseMap();
        CreateMap<User, UpdateCommand>().ReverseMap();
        CreateMap<User, UpdateEmailCommand>().ReverseMap();
        CreateMap<User, UpdateUsernameCommand>().ReverseMap();
    }
}
