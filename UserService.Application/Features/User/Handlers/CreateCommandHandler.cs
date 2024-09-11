using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Application.DTOs;
using UserService.Application.Features.User.Commands;
using UserService.Domain.Interfaces;
using Entity = UserService.Domain.Entities;

namespace UserService.Application.Features.User.Handlers;

public class CreateCommandHandler(
    IMapper mapper,
    IUserRepository userRepository,
    IPasswordHasher<Entity.User> passwordHasher
) : IRequestHandler<CreateCommand, UserResponseDTO>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher<Entity.User> _passwordHasher = passwordHasher;

    public async Task<UserResponseDTO> Handle(
        CreateCommand request,
        CancellationToken cancellationToken
    )
    {
        request.Password = _passwordHasher.HashPassword(new Entity.User(), request.Password);
        var user = await _userRepository.RegisterAsync(_mapper.Map<Entity.User>(request));
        var userResponse = _mapper.Map<UserResponseDTO>(user);
        return userResponse;
    }
}
