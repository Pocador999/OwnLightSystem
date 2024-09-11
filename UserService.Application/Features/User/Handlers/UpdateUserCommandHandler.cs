using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Features.User.Commands;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.User.Handlers;

public class UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    : IRequestHandler<UpdateUserCommand, UserResponseDTO>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<UserResponseDTO> Handle(
        UpdateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var user =
            await _userRepository.FindByIdAsync(request.Id)
            ?? throw new Exception("User not found");
        _mapper.Map(request, user);
        user.UpdatedAt = DateTime.UtcNow;
        var updatedUser = await _userRepository.UpdateAsync(user);
        return _mapper.Map<UserResponseDTO>(updatedUser);
    }
}
