using AutoMapper;
using MediatR;
using UserService.Application.DTOs;
using UserService.Application.Features.User.Commands;
using UserService.Domain.Interfaces;

namespace UserService.Application.Features.User.Handlers;

public class UpdateCommandHandler(IUserRepository userRepository)
    : IRequestHandler<UpdateCommand>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Unit> Handle(
        UpdateCommand request,
        CancellationToken cancellationToken
    )
    {
        var user =
            await _userRepository.FindByIdAsync(request.Id)
            ?? throw new Exception("User not found");
        user.UpdatedAt = DateTime.UtcNow;
        user.UpdateUser(request.Name, request.UserName);
        await _userRepository.UpdateAsync(user);
        return Unit.Value;
    }
}
